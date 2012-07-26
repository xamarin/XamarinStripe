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
    public class StripeFeeDetail {
        [JsonProperty (PropertyName = "type")]
        string Type { get; set; }
        [JsonProperty (PropertyName = "currency")]
        string Currency { get; set; }
        [JsonProperty (PropertyName = "application")]
        string Application { get; set; }
        [JsonProperty (PropertyName = "description")]
        string Description { get; set; }
        [JsonProperty (PropertyName = "amount")]
        public int Amount { get; set; }
    }
}
