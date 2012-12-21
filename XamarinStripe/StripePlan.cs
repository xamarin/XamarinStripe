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
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripePlan : StripeId {
        [JsonProperty (PropertyName = "livemode")]
        public bool LiveMode { get; set; }

        [JsonProperty (PropertyName = "amount")]
        public int Amount { get; set; }

        [JsonProperty (PropertyName = "currency")]
        public string Currency { get; set; }

        // deleted was deleted
        /*
        [JsonProperty(PropertyName = "deleted")]
        public bool? Deleted { get; set; }
        */

        [JsonProperty(PropertyName = "identifier")]
        public string Identifier { get; set; }

        [JsonProperty (PropertyName = "interval")]
        public StripePlanInterval Interval { get; set; }

        [JsonProperty (PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty (PropertyName = "trial_period_days")]
        public int? TrialPeriod { get; set; }
    }
}
