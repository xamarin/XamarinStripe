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
using System.Globalization;
using System.Text;
using System.Web;

namespace Xamarin.Payments.Stripe {
    public class StripeCustomerInfo : IUrlEncoderInfo {
        public StripeCreditCardInfo Card { get; set; }
        // The strings allow ""
        public string Coupon { get; set; }
        public string Email { get; set; } //TODO: Validate this using System.Net.Mail.MailAddress?
        public string Description { get; set; }
        public string Plan { get; set; }
        public DateTime? TrialEnd { get; set; }
        public bool? Validate { get; set; }
        public string DefaultCardId { get; set; }

        public virtual void UrlEncode (StringBuilder sb)
        {
            if (Card != null)
                Card.UrlEncode (sb);
            if (Coupon != null)
                sb.AppendFormat ("coupon={0}&", HttpUtility.UrlEncode (Coupon));
            if (Email != null)
                sb.AppendFormat ("email={0}&", HttpUtility.UrlEncode (Email));
            if (Description != null)
                sb.AppendFormat ("description={0}&", HttpUtility.UrlEncode (Description));
            if (Plan != null)
                sb.AppendFormat ("plan={0}&", HttpUtility.UrlEncode (Plan));
            if (TrialEnd.HasValue)
                sb.AppendFormat ("trial_end={0}&", TrialEnd.Value.ToUnixEpoch ());
            if (Validate.HasValue)
                sb.AppendFormat ("validate={0}&", Validate.Value.ToString (CultureInfo.InvariantCulture).ToLowerInvariant ());
            if (DefaultCardId != null)
                sb.AppendFormat ("default_card={0}&", HttpUtility.UrlEncode (DefaultCardId));
        }
    }
}
