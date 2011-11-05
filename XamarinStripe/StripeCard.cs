/*
 * Copyright 2011 Xamarin, Inc.
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
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripeCard {
        [JsonProperty (PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty (PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty (PropertyName = "exp_month")]
        public int ExpirationMonth { get; set; }
        [JsonProperty (PropertyName = "exp_year")]
        public int ExpirationYear { get; set; }
        [JsonProperty (PropertyName = "last4")]
        public string Last4 { get; set; }
        [JsonProperty (PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty (PropertyName = "cvc_check")]
        public StripeCvcCheck CvcCheck { get; set; }
        [JsonProperty (PropertyName = "object")]
        public StripeObject Object { get; set; }
    }
}
