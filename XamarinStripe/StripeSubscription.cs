/*
 * Copyright 2011, 2012 Joe Dluzen, 2012 Xamarin, Inc.
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
    public class StripeSubscription: StripeObject {
        [JsonProperty (PropertyName = "current_period_end")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? CurrentPeriodEnd { get; set; }

        [JsonProperty (PropertyName = "status")]
        [JsonConverter (typeof(StripeEnumConverter<StripeSubscriptionStatus>))]
        public StripeSubscriptionStatus Status { get; set; }

        [JsonProperty (PropertyName = "plan")]
        public StripePlan Plan { get; set; }

        [JsonProperty (PropertyName = "current_period_start")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? CurrentPeriodStart { get; set; }

        [JsonProperty (PropertyName = "start")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? Start { get; set; }

        [JsonProperty (PropertyName = "trial_start")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? TrialStart { get; set; }

        [JsonProperty (PropertyName = "cancel_at_period_end")]
        public bool? CancelAtPeriodEnd { get; set; }

        [JsonProperty (PropertyName = "trial_end")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? TrialEnd { get; set; }

        [JsonProperty (PropertyName = "canceled_at")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? CanceledAt { get; set; }

        [JsonProperty (PropertyName = "ended_at")]
        [JsonConverter (typeof(UnixDateTimeConverter))]
        public DateTime? EndedAt { get; set; }

        [JsonProperty (PropertyName = "customer")]
        public string CustomerID { get; set; }
    }
}
