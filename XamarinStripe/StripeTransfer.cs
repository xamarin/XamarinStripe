/*
 * Copyright 2012 Xamarin, Inc.
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
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripeTransfer : StripeId {
        [JsonProperty (PropertyName = "amount")]
        public int Amount { get; set; }

        [JsonProperty (PropertyName = "date")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty (PropertyName = "transactions")]
        public StripeCollection<StripeLineItem> Transactions { get; set; }

        [JsonProperty (PropertyName = "other_transfers")]
        public List<string> OtherTransfers { get; set; }

        [JsonProperty (PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty (PropertyName = "summary")]
        public TransferSummary Summary { get; set; }
        
        public class TransferSummary {
            [JsonProperty (PropertyName = "adjustment_count")]
            public int AdjustmentCount { get; set; }

            [JsonProperty (PropertyName = "adjustment_fee_details")]
            public List<StripeFeeDetail> AdjustmentFeeDetails { get; set; }

            [JsonProperty (PropertyName = "adjustment_fees")]
            public int AdjustmentFees { get; set; }

            [JsonProperty (PropertyName = "adjustment_gross")]
            public int AdjustmentGross { get; set; }

            [JsonProperty (PropertyName = "charge_count")]
            public int ChargeCount { get; set; }

            [JsonProperty (PropertyName = "charge_fee_details")]
            public List<StripeFeeDetail> ChargeFeeDetails { get; set; }

            [JsonProperty (PropertyName = "charge_fees")]
            public int ChargeFees { get; set; }

            [JsonProperty (PropertyName = "charge_gross")]
            public int ChargeGross { get; set; }

            [JsonProperty (PropertyName = "currency")]
            public string Currency { get; set; }

            [JsonProperty (PropertyName = "net")]
            public int Net { get; set; }

            [JsonProperty (PropertyName = "refund_count")]
            public int RefundCount { get; set; }

            [JsonProperty (PropertyName = "refund_fees")]
            public int RefundFees { get; set; }

            [JsonProperty (PropertyName = "refund_gross")]
            public int RefundGross { get; set; }

            [JsonProperty (PropertyName = "validation_count")]
            public int ValidationCount { get; set; }

            [JsonProperty (PropertyName = "validation_fees")]
            public int ValidationFees { get; set; }
        }

        [JsonProperty (PropertyName = "description")]
        public string Description { get; set; }
    }
}
