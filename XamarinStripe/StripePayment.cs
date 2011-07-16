/*
 * Copyright 2011 Xamarin, Inc.
 *
 * Author(s):
 * 	Gonzalo Paniagua Javier (gonzalo@xamarin.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    public class StripePayment {
        static string api_endpoint = "https://api.stripe.com/v1";
        static string user_agent = "Stripe .NET v1";
        static Encoding encoding = Encoding.UTF8;

        ICredentials credential;

        public StripePayment (string api_key)
        {
            credential = new NetworkCredential (api_key, "");
            TimeoutSeconds = 30;
        }

        protected virtual WebRequest SetupRequest (string method, string url)
        {
            WebRequest req = (WebRequest) WebRequest.Create (url);
            req.Method = method;
            if (req is HttpWebRequest) {
                ((HttpWebRequest) req).UserAgent = user_agent;
            }
            req.Credentials = credential;
            req.PreAuthenticate = true;
            req.Timeout = TimeoutSeconds * 1000;
            // Perhaps we should only set this for POST. DELETE and GET don't need it.
            req.ContentType = "application/x-www-form-urlencoded";
            return req;
        }

        static string GetResponseAsString (WebResponse response)
        {
            using (StreamReader sr = new StreamReader (response.GetResponseStream (), encoding)) {
                return sr.ReadToEnd ();
            }
        }

        protected virtual string DoRequest (string endpoint, string method, string body)
        {
            string result = null;
            WebRequest req = SetupRequest (method, endpoint);
            if (body != null) {
                byte [] bytes = encoding.GetBytes (body.ToString ());
                req.ContentLength = bytes.Length;
                using (Stream st = req.GetRequestStream ()) {
                    st.Write (bytes, 0, bytes.Length);
                }
            }

            try {
                using (WebResponse resp = (WebResponse) req.GetResponse ()) {
                    result = GetResponseAsString (resp);
                }
            } catch (WebException wexc) {
                if (wexc.Response != null) {
                    string json_error = GetResponseAsString (wexc.Response);
                    HttpStatusCode status_code = HttpStatusCode.BadRequest;
                    HttpWebResponse resp = wexc.Response as HttpWebResponse;
                    if (resp != null)
                        status_code = resp.StatusCode;

                    if ((int)status_code <= 500)
                        throw StripeException.GetFromJSON (status_code, json_error);
                }
                throw;
            }
            return result;
        }

        #region Charge
        public StripeCharge Charge (int amount_cents, string currency, string customer, string description)
        {
            if (String.IsNullOrEmpty (customer))
                throw new ArgumentNullException ("customer");

            return Charge (amount_cents, currency, customer, null, description);
        }

        public StripeCharge Charge (int amount_cents, string currency, StripeCreditCardInfo card, string description)
        {
            if (card == null)
                throw new ArgumentNullException ("card");

            return Charge (amount_cents, currency, null, card, description);
        }

        StripeCharge Charge (int amount_cents, string currency, string customer, StripeCreditCardInfo card, string description)
        {
            if (amount_cents < 0)
                throw new ArgumentOutOfRangeException ("amount_cents", "Must be greater than or equal 0");
            if (String.IsNullOrEmpty (currency))
                throw new ArgumentNullException ("currency");
            if (currency != "usd")
                throw new ArgumentException ("The only supported currency is 'usd'");

            StringBuilder str = new StringBuilder ();
            str.AppendFormat ("amount={0}&", amount_cents);
            str.AppendFormat ("currency={0}&", currency);
            if (!String.IsNullOrEmpty (description)) {
                str.AppendFormat ("description={0}&", HttpUtility.UrlEncode (description));
            }

            if (card != null) {
                card.UrlEncode (str);
            } else {
                // customer is non-empty
                str.AppendFormat ("customer={0}&", HttpUtility.UrlEncode (customer));
            }
            str.Length--;
            string ep = String.Format ("{0}/charges", api_endpoint);
            string json = DoRequest (ep, "POST", str.ToString ());
            return JsonConvert.DeserializeObject<StripeCharge> (json);
        }

        public StripeCharge GetCharge (string charge_id)
        {
            if (String.IsNullOrEmpty (charge_id))
                throw new ArgumentNullException ("charge_id");

            string ep = String.Format ("{0}/charges/{1}", api_endpoint, HttpUtility.UrlEncode (charge_id));
            string json = DoRequest (ep, "GET", null);
            return JsonConvert.DeserializeObject<StripeCharge> (json);
        }

        public List<StripeCharge> GetCharges ()
        {
            return GetCharges (0, 10);
        }

        public List<StripeCharge> GetCharges (int offset, int count)
        {
            return GetCharges (offset, count, null);
        }

        public List<StripeCharge> GetCharges (int offset, int count, string customer_id)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException ("count");

            StringBuilder str = new StringBuilder ();
            str.AppendFormat ("offset={0}&", offset);
            str.AppendFormat ("count={0}&", count);
            if (!String.IsNullOrEmpty (customer_id))
                str.AppendFormat ("customer={0}&", HttpUtility.UrlEncode (customer_id));

            str.Length--;
            string ep = String.Format ("{0}/charges?{1}", api_endpoint, str);
            string json = DoRequest (ep, "GET", null);
            return JsonConvert.DeserializeObject<List<StripeCharge>> (json);
        }
        #endregion
        #region Refund
        public StripeCharge Refund (string charge_id)
        {
            if (String.IsNullOrEmpty (charge_id))
                throw new ArgumentNullException ("charge_id");

            string ep = String.Format ("{0}/charges/{1}/refund", api_endpoint, HttpUtility.UrlEncode (charge_id));
            string json = DoRequest (ep, "POST", "");
            return JsonConvert.DeserializeObject<StripeCharge> (json);
        }
        #endregion
        #region Customer
        StripeCustomer CreateOrUpdateCustomer (string id, StripeCustomerInfo customer)
        {
            StringBuilder str = new StringBuilder ();
            customer.UrlEncode (str);
            if (str.Length > 0)
                str.Length--;

            string format = "{0}/customers"; // Create
            if (id != null)
                format = "{0}/customers/{1}"; // Update
            string ep = String.Format (format, api_endpoint, HttpUtility.UrlEncode (id));
            string json = DoRequest (ep, "POST", str.ToString ());
            return JsonConvert.DeserializeObject<StripeCustomer> (json);
        }

        public StripeCustomer CreateCustomer (StripeCustomerInfo customer)
        {
            if (customer == null)
                throw new ArgumentNullException ("customer");

            return CreateOrUpdateCustomer (null, customer);
        }

        public StripeCustomer UpdateCustomer (string id, StripeCustomerInfo customer)
        {
            if (String.IsNullOrEmpty (id))
                throw new ArgumentNullException ("id");
            if (customer == null)
                throw new ArgumentNullException ("customer");

            return CreateOrUpdateCustomer (id, customer);
        }

        public StripeCustomer GetCustomer (string customer_id)
        {
            if (String.IsNullOrEmpty (customer_id))
                throw new ArgumentNullException ("customer_id");

            string ep = String.Format ("{0}/customers/{1}", api_endpoint, HttpUtility.UrlEncode (customer_id));
            string json = DoRequest (ep, "GET", null);
            return JsonConvert.DeserializeObject<StripeCustomer> (json);
        }

        public List<StripeCustomer> GetCustomers ()
        {
            return GetCustomers (0, 10);
        }

        public List<StripeCustomer> GetCustomers (int offset, int count)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException ("count");

            string str = String.Format ("offset={0}&count={1}", offset, count);
            string ep = String.Format ("{0}/customers?{1}", api_endpoint, str);
            string json = DoRequest (ep, "GET", null);
            return JsonConvert.DeserializeObject<List<StripeCustomer>> (json);
        }

        public StripeCustomer DeleteCustomer (string customer_id)
        {
            if (String.IsNullOrEmpty (customer_id))
                throw new ArgumentNullException ("customer_id");

            string ep = String.Format ("{0}/customers/{1}", api_endpoint, HttpUtility.UrlEncode (customer_id));
            string json = DoRequest (ep, "DELETE", null);
            return JsonConvert.DeserializeObject<StripeCustomer> (json);
        }
        #endregion

        public int TimeoutSeconds { get; set; }
    }
}
