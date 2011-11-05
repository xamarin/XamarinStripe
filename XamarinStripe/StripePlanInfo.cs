/*
 * Copyright 2011 Joe Dluzen
 *
 * Author(s):
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
using System.Text;
using System.Web;

namespace Xamarin.Payments.Stripe {
    public class StripePlanInfo : IUrlEncoderInfo {
        public string ID { get; set; }

        public int Amount { get; set; }

        public string Currency { get; set; }

        public StripeInterval Interval { get; set; }

        public string Name { get; set; }

        public int TrialPeriod { get; set; }

        #region IUrlEncoderInfo Members

        public void UrlEncode (StringBuilder sb)
        {
            sb.AppendFormat ("id={0}&amount={1}&currency={2}&interval={3}&name={4}&",
                HttpUtility.UrlEncode (ID), Amount, HttpUtility.UrlEncode (Currency ?? "usd"), HttpUtility.UrlEncode (Interval.ToString().ToLower ()), HttpUtility.UrlEncode (Name));
            if (TrialPeriod > 0)
                sb.AppendFormat ("trial_period_days={0}&", TrialPeriod);
        }

        #endregion
    }
}
