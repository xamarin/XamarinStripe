/*
 * Copyright 2011 Joe Dluzen, 2012 Xamarin, Inc.
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
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripeLineItem : StripeId {
        [JsonProperty ("livemode")]
        public bool LiveMode { get; set; }
        [JsonProperty ("type")]
        public string Type { get; set; }
        [JsonProperty ("amount")]
        public int Amount { get; set; }
        [JsonProperty ("period")]
        public StripePeriod Period { get; set; }
        [JsonProperty ("proration")]
        public bool Proration { get; set; }
        [JsonProperty ("quantity")]
        public int? Quantity { get; set; }
        [JsonProperty ("description")]
        public string Description { get; set; }
        [JsonProperty ("currency")]
        public string Currency { get; set; }
        [JsonProperty ("plan")]
        public StripePlan Plan { get; set; }
    }
}
