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

namespace Xamarin.Payments.Stripe
{
    public class StripeCouponInfo : IUrlEncoderInfo
    {
        public string ID { get; set; }

        public int PercentOff { get; set; }

        public int AmmountOff { get; set; }

        public StripeCouponDuration Duration { get; set; }

        public int MonthsForDuration { get; set; }

        public int MaxRedemptions { get; set; }

        public DateTime RedeemBy { get; set; }

        #region IUrlEncoderInfo implementation
        public virtual void UrlEncode(StringBuilder sb)
        {
            if (AmmountOff > 0)
            {
                sb.AppendFormat("amount_off={0}", AmmountOff);
            }
            else
            {
                sb.AppendFormat("percent_off={0}", PercentOff);
            }
            sb.AppendFormat("&duration={0}&", HttpUtility.UrlEncode(Duration.ToString().ToLower()));
            if (!string.IsNullOrEmpty(ID))
                sb.AppendFormat("id={0}&", HttpUtility.UrlEncode(ID));
            if (MonthsForDuration > 0)
                sb.AppendFormat("duration_in_months={0}&", MonthsForDuration);
            if (MaxRedemptions > 0)
                sb.AppendFormat("max_redemptions={0}&", MaxRedemptions);
            if (RedeemBy != DateTime.MinValue)
                sb.AppendFormat("redeem_by={0}&", RedeemBy.ToUnixEpoch());
        }
        #endregion
    }
}
