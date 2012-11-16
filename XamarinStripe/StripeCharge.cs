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
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripeCharge : StripeId {
        [JsonProperty (PropertyName = "attempted")]
        public bool Attempted { get; set; }
        [JsonProperty (PropertyName = "refunded")]
        public bool Refunded { get; set; }
        [JsonProperty (PropertyName = "amount_refunded")]
        public int AmountRefunded { get; set; }
        [JsonProperty (PropertyName = "paid")]
        public bool Paid { get; set; }
        [JsonProperty (PropertyName = "amount")]
        public int Amount { get; set; }
        [JsonProperty (PropertyName = "fee")]
        public int Fee { get; set; }
        [JsonProperty (PropertyName = "livemode")]
        public bool LiveMode { get; set; }
        /* api changed */
        /*
        [JsonProperty (PropertyName = "disputed")]
        public bool Disputed { get; set; }
         */
        [JsonProperty (PropertyName = "dispute")]
        public StripeDispute Dispute { get; set; }
        [JsonProperty (PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty (PropertyName = "currency")]
        public string Currency { get; set; }
        [JsonProperty (PropertyName = "created")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime? Created { get; set; }
        [JsonProperty (PropertyName = "card")]
        public StripeCard Card { get; set; }
        [JsonProperty (PropertyName = "customer")]
        public string Customer { get; set; }
        [JsonProperty (PropertyName = "fee_details")]
        public StripeFeeDetail [] FeeDetails { get; set; }
        [JsonProperty (PropertyName = "failure_message")]
        public string FailureMessage { get; set; }
        [JsonProperty (PropertyName = "invoice")]
        public string Invoice { get; set; }
    }
}
