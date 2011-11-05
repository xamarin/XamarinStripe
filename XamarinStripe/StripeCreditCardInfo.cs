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
using System.Text;
using System.Web;

namespace Xamarin.Payments.Stripe {
    public class StripeCreditCardInfo : IUrlEncoderInfo {
        // Mandatory
        public string Number { get; set; }
        // Mandatory when Number is a not one-time token instead of a number
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        // Optional, highly recommended
        public string CVC { get; set; }
        // Optional, recommended
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }

        public virtual void UrlEncode (StringBuilder sb)
        {
            if (String.IsNullOrEmpty (Number))
                throw new ArgumentNullException ("Number");

            bool is_number = Char.IsDigit (Number [0]);
            if (is_number && (ExpirationMonth <= 0 || ExpirationMonth > 12))
                throw new ArgumentOutOfRangeException ("ExpirationMonth");
            if (is_number && ExpirationYear <= 0)
                throw new ArgumentOutOfRangeException ("ExpirationYear");

            if (!is_number) {
                // One-time token
                sb.AppendFormat ("card={0}&", HttpUtility.UrlEncode (Number));
                return;
            }
            sb.AppendFormat ("card[number]={0}&card[exp_month]={1}&card[exp_year]={2}&",
                            HttpUtility.UrlEncode (Number), ExpirationMonth, ExpirationYear);
            if (!String.IsNullOrEmpty (CVC))
                sb.AppendFormat ("card[cvc]={0}&", HttpUtility.UrlEncode (CVC));
            if (!String.IsNullOrEmpty (FullName))
                sb.AppendFormat ("card[name]={0}&", HttpUtility.UrlEncode (FullName));
            if (!String.IsNullOrEmpty (AddressLine1))
                sb.AppendFormat ("card[address_line1]={0}&", HttpUtility.UrlEncode (AddressLine1));
            if (!String.IsNullOrEmpty (AddressLine2))
                sb.AppendFormat ("card[address_line2]={0}&", HttpUtility.UrlEncode (AddressLine2));
            if (!String.IsNullOrEmpty (ZipCode))
                sb.AppendFormat ("card[address_zip]={0}&", HttpUtility.UrlEncode (ZipCode));
            if (!String.IsNullOrEmpty (StateOrProvince))
                sb.AppendFormat ("card[address_state]={0}&", HttpUtility.UrlEncode (StateOrProvince));
            if (!String.IsNullOrEmpty (Country))
                sb.AppendFormat ("card[address_country]={0}&", HttpUtility.UrlEncode (Country));
        }
    }
}
