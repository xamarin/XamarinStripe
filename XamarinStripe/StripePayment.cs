/*
 * Copyright 2011 - 2012 Xamarin, Inc., 2011 - 2012 Joe Dluzen
 *
 * Author(s):
 *  Gonzalo Paniagua Javier (gonzalo@xamarin.com)
 *  Joe Dluzen (jdluzen@gmail.com)
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
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    public class StripePayment {
        static readonly string api_endpoint = "https://api.stripe.com/v1";
        static readonly string subscription_path = "{0}/customers/{1}/subscription";
        static readonly string user_agent = "Stripe .NET v1";
        static readonly Encoding encoding = Encoding.UTF8;
        ICredentials credential;

        public StripePayment (string api_key)
        {
            credential = new NetworkCredential (api_key, "");
            TimeoutSeconds = 80;
        }
        #region Shared
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
            if (method == "POST")
                req.ContentType = "application/x-www-form-urlencoded";
            return req;
        }

        static string GetResponseAsString (WebResponse response)
        {
            using (StreamReader sr = new StreamReader (response.GetResponseStream (), encoding)) {
                return sr.ReadToEnd ();
            }
        }

        protected virtual T DoRequest<T> (string endpoint, string method = "GET", string body = null)
        {
            var json = DoRequest (endpoint, method, body);
            return JsonConvert.DeserializeObject<T> (json);
        }

        protected virtual string DoRequest (string endpoint)
        {
            return DoRequest (endpoint, "GET", null);
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

                    if ((int) status_code <= 500)
                        throw StripeException.GetFromJSON (status_code, json_error);
                }
                throw;
            }
            return result;
        }

        protected virtual StringBuilder UrlEncode (IUrlEncoderInfo infoInstance)
        {
            StringBuilder str = new StringBuilder ();
            infoInstance.UrlEncode (str);
            if (str.Length > 0)
                str.Length--;
            return str;
        }

        #endregion
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
            return DoRequest<StripeCharge> (ep, "POST", str.ToString ());
        }

        public StripeCharge GetCharge (string charge_id)
        {
            if (String.IsNullOrEmpty (charge_id))
                throw new ArgumentNullException ("charge_id");

            string ep = String.Format ("{0}/charges/{1}", api_endpoint, HttpUtility.UrlEncode (charge_id));
            return DoRequest<StripeCharge> (ep);
        }

        public StripeCollection<StripeCharge> GetCharges (int offset = 0, int count = 10, string customer_id = null)
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
            return DoRequest<StripeCollection<StripeCharge>> (ep);
        }

        public StripeDispute UpdateDispute (string charge_id, string evidence)
        {
            if (String.IsNullOrEmpty (charge_id))
                throw new ArgumentNullException ("charge_id");

            if (String.IsNullOrEmpty (evidence))
                throw new ArgumentNullException ("evidence");

            string ep = String.Format ("{0}/charges/{1}/dispute", api_endpoint, HttpUtility.UrlEncode (charge_id));
            return DoRequest<StripeDispute> (ep, "POST", String.Format ("evidence={0}", HttpUtility.UrlEncode (evidence)));
        }

        #endregion
        #region Refund
        public StripeCharge Refund (string charge_id)
        {
            if (String.IsNullOrEmpty (charge_id))
                throw new ArgumentNullException ("charge_id");

            string ep = String.Format ("{0}/charges/{1}/refund", api_endpoint, HttpUtility.UrlEncode (charge_id));
            return DoRequest<StripeCharge> (ep, "POST", null);
        }

        public StripeCharge Refund (string charge_id, int amount)
        {
            if (String.IsNullOrEmpty (charge_id))
                throw new ArgumentNullException ("charge_id");
            if (amount <= 0)
                throw new ArgumentException ("Amount must be greater than zero.", "amount");

            string ep = String.Format ("{0}/charges/{1}/refund?amount={2}", api_endpoint, HttpUtility.UrlEncode (charge_id), amount);
            return DoRequest<StripeCharge> (ep, "POST", null);
        }
        #endregion
        #region Customer
        StripeCustomer CreateOrUpdateCustomer (string id, StripeCustomerInfo customer)
        {
            StringBuilder str = UrlEncode (customer);

            string format = "{0}/customers"; // Create
            if (id != null)
                format = "{0}/customers/{1}"; // Update
            string ep = String.Format (format, api_endpoint, HttpUtility.UrlEncode (id));
            return DoRequest<StripeCustomer> (ep, "POST", str.ToString ());
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
            return DoRequest<StripeCustomer> (ep);
        }

        public StripeCollection<StripeCustomer> GetCustomers (int offset = 0, int count = 10)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException ("count");

            string str = String.Format ("offset={0}&count={1}", offset, count);
            string ep = String.Format ("{0}/customers?{1}", api_endpoint, str);
            return DoRequest<StripeCollection<StripeCustomer>> (ep);
        }

        public StripeCustomer DeleteCustomer (string customer_id)
        {
            if (String.IsNullOrEmpty (customer_id))
                throw new ArgumentNullException ("customer_id");

            string ep = String.Format ("{0}/customers/{1}", api_endpoint, HttpUtility.UrlEncode (customer_id));
            return DoRequest<StripeCustomer> (ep, "DELETE", null);
        }
        #endregion
        #region Events
        public StripeEvent GetEvent (string eventId)
        {
            string ep = string.Format ("{0}/events/{1}", api_endpoint, HttpUtility.UrlEncode (eventId));
            return DoRequest<StripeEvent> (ep);
        }

        public StripeCollection<StripeEvent> GetEvents (int offset = 0, int count = 10, string type = null)
        {
            // NOTE: we aren't currnently mapping created
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException ("count");

            string args = String.Format ("offset={0}&count={1}", offset, count);

            if (!string.IsNullOrEmpty (type))
                args += String.Format ("&type={0}", HttpUtility.UrlEncode (type));

            string ep = String.Format ("{0}/events?{1}", api_endpoint, args);
            return DoRequest<StripeCollection<StripeEvent>> (ep);
        }

        #endregion

        #region Tokens
        public StripeCreditCardToken CreateToken (StripeCreditCardInfo card)
        {
            if (card == null)
                throw new ArgumentNullException ("card");
            StringBuilder str = UrlEncode (card);

            string ep = string.Format ("{0}/tokens", api_endpoint);
            return DoRequest<StripeCreditCardToken> (ep, "POST", str.ToString ());
        }

        public StripeCreditCardToken GetToken (string tokenId)
        {
            if (string.IsNullOrEmpty (tokenId))
                throw new ArgumentNullException (tokenId);

            string ep = string.Format ("{0}/tokens/{1}", api_endpoint, HttpUtility.UrlEncode (tokenId));
            return DoRequest<StripeCreditCardToken> (ep);
        }
        #endregion
        #region Plans
        public StripePlan CreatePlan (StripePlanInfo plan)
        {
            if (plan == null)
                throw new ArgumentNullException ("plan");
            StringBuilder str = UrlEncode (plan);

            string ep = string.Format ("{0}/plans", api_endpoint);
            return DoRequest<StripePlan> (ep, "POST", str.ToString ());
        }

        public StripePlan GetPlan (string planId)
        {
            if (string.IsNullOrEmpty (planId))
                throw new ArgumentNullException ("id");

            string ep = string.Format ("{0}/plans/{1}", api_endpoint, HttpUtility.UrlEncode (planId));
            return DoRequest<StripePlan> (ep);
        }

        public StripePlan DeletePlan (string planId)
        {
            if (string.IsNullOrEmpty (planId))
                throw new ArgumentNullException ("id");

            string ep = string.Format ("{0}/plans/{1}", api_endpoint, HttpUtility.UrlEncode (planId));
            return DoRequest<StripePlan> (ep, "DELETE", null);
        }

        public StripeCollection<StripePlan> GetPlans (int offset = 0, int count = 10)
        {
            string str = string.Format ("count={0}&offset={1}", count, offset);
            string ep = string.Format ("{0}/plans?{1}", api_endpoint, str);
            return DoRequest<StripeCollection<StripePlan>> (ep);
        }
        #endregion
        #region Subscriptions
        public StripeSubscription Subscribe (string customerId, StripeSubscriptionInfo subscription)
        {
            StringBuilder str = UrlEncode (subscription);
            string ep = string.Format (subscription_path, api_endpoint, HttpUtility.UrlEncode (customerId));
            return DoRequest<StripeSubscription> (ep, "POST", str.ToString ());
        }

        public StripeSubscription GetSubscription (string customerId)
        {
            if (string.IsNullOrEmpty (customerId))
                throw new ArgumentNullException ("customerId");
            string ep = string.Format (subscription_path, api_endpoint, HttpUtility.UrlEncode (customerId));
            return DoRequest<StripeSubscription>(ep);
        }

        public StripeSubscription Unsubscribe (string customerId, bool atPeriodEnd)
        {
            string ep = string.Format (subscription_path + "?at_period_end={2}", api_endpoint, HttpUtility.UrlEncode (customerId), atPeriodEnd.ToString (CultureInfo.InvariantCulture).ToLowerInvariant ());
            return DoRequest<StripeSubscription> (ep, "DELETE", null);
        }
        #endregion
        #region Invoice items
        public StripeInvoiceItem CreateInvoiceItem (StripeInvoiceItemInfo item)
        {
            if (string.IsNullOrEmpty (item.CustomerID))
                throw new ArgumentNullException ("item.CustomerID");
            StringBuilder str = UrlEncode (item);
            string ep = string.Format ("{0}/invoiceitems", api_endpoint);
            return DoRequest<StripeInvoiceItem> (ep, "POST", str.ToString ());
        }

        public StripeInvoiceItem GetInvoiceItem (string invoiceItemId)
        {
            if (string.IsNullOrEmpty (invoiceItemId))
                throw new ArgumentNullException ("invoiceItemId");
            string ep = string.Format ("{0}/invoiceitems/{1}", api_endpoint, invoiceItemId);
            return DoRequest<StripeInvoiceItem> (ep);
        }

        public StripeInvoiceItem UpdateInvoiceItem (string invoiceItemId, StripeInvoiceItemInfo item)
        {
            StringBuilder str = UrlEncode (item);
            string ep = string.Format ("{0}/invoiceitems/{1}", api_endpoint, invoiceItemId);
            return DoRequest<StripeInvoiceItem> (ep, "POST", str.ToString ());
        }

        public StripeInvoiceItem DeleteInvoiceItem (string invoiceItemId)
        {
            string ep = string.Format ("{0}/invoiceitems/{1}", api_endpoint, invoiceItemId);
            return DoRequest<StripeInvoiceItem> (ep, "DELETE", null);
        }

        public StripeCollection<StripeInvoiceItem> GetInvoiceItems (int offset = 0, int count = 10, string customerId = null)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException ("count");

            StringBuilder str = new StringBuilder ();
            str.AppendFormat ("offset={0}&", offset);
            str.AppendFormat ("count={0}&", count);
            if (!string.IsNullOrEmpty (customerId))
                str.AppendFormat ("customer={0}&", HttpUtility.UrlEncode (customerId));

            str.Length--;
            string ep = String.Format ("{0}/invoiceitems?{1}", api_endpoint, str);
            return DoRequest<StripeCollection<StripeInvoiceItem>> (ep);
        }

        #endregion
        #region Invoices
        public StripeInvoice GetInvoice (string invoiceId)
        {
            if (string.IsNullOrEmpty (invoiceId))
                throw new ArgumentNullException ("invoiceId");
            string ep = string.Format ("{0}/invoices/{1}", api_endpoint, invoiceId);
            return DoRequest<StripeInvoice> (ep);
        }
        
        public StripeCollection<StripeInvoice> GetInvoices (int offset = 0, int count = 10, string customerId = null)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException ("count");

            StringBuilder str = new StringBuilder ();
            str.AppendFormat ("offset={0}&", offset);
            str.AppendFormat ("count={0}&", count);
            if (!string.IsNullOrEmpty (customerId))
                str.AppendFormat ("customer={0}&", HttpUtility.UrlEncode (customerId));

            str.Length--;
            string ep = String.Format ("{0}/invoices?{1}", api_endpoint, str);
            return DoRequest<StripeCollection<StripeInvoice>>(ep);
        }

        public StripeInvoice GetUpcomingInvoice (string customerId)
        {
            if (string.IsNullOrEmpty (customerId))
                throw new ArgumentOutOfRangeException ("customerId");
            string ep = String.Format ("{0}/invoices/upcoming?customer={1}", api_endpoint, customerId);
            return DoRequest<StripeInvoice> (ep);
        }

        public StripeCollection<StripeLineItem> GetInvoiceLines (string invoiceId)
        {
            if (string.IsNullOrEmpty (invoiceId))
                throw new ArgumentNullException ("invoiceId");
            string ep = string.Format ("{0}/invoices/{1}/lines", api_endpoint, invoiceId);
            return DoRequest<StripeCollection<StripeLineItem>> (ep);
        }
        #endregion
        #region Coupons
        public StripeCoupon CreateCoupon (StripeCouponInfo coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException ("coupon");
            if (coupon.PercentOff < 1 || coupon.PercentOff > 100)
                throw new ArgumentOutOfRangeException ("coupon.PercentOff");
            if (coupon.Duration == StripeCouponDuration.Repeating && coupon.MonthsForDuration < 1)
                throw new ArgumentException ("MonthsForDuration must be greater than 1 when Duration = Repeating");
            StringBuilder str = UrlEncode (coupon);
            string ep = string.Format ("{0}/coupons", api_endpoint);
            return DoRequest<StripeCoupon> (ep, "POST", str.ToString ());
        }

        public StripeCoupon GetCoupon (string couponId)
        {
            if (string.IsNullOrEmpty (couponId))
                throw new ArgumentNullException ("couponId");
            string ep = string.Format ("{0}/coupons/{1}", api_endpoint, couponId);
            return DoRequest<StripeCoupon> (ep);
        }

        public StripeCoupon DeleteCoupon (string couponId)
        {
            if (string.IsNullOrEmpty (couponId))
                throw new ArgumentNullException ("couponId");
            string ep = string.Format ("{0}/coupons/{1}", api_endpoint, couponId);
            return DoRequest<StripeCoupon> (ep, "DELETE", null);
        }

        public StripeCollection<StripeCoupon> GetCoupons (int offset = 0, int count = 10)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException ("offset");
            if (count > 100)
                throw new ArgumentOutOfRangeException ("count");
            string ep = string.Format ("{0}/coupons?offset={0}&count={1}", api_endpoint, offset, count);
            return DoRequest<StripeCollection<StripeCoupon>> (ep);
        }
        #endregion
        public int TimeoutSeconds { get; set; }
    }
}
