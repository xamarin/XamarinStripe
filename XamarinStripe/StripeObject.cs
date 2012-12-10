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
using Newtonsoft.Json.Converters;

namespace Xamarin.Payments.Stripe {
    public class StripeObject {
        [JsonProperty (PropertyName = "object")]
        public StripeObjectType Object { get; set; }
    }

    public class StripeId : StripeObject {
        [JsonProperty (PropertyName = "id")] 
        public string ID { get; set; }
    }

    [JsonConverter (typeof (StripeEnumConverter<StripeObjectType>))]
    public enum StripeObjectType {
        Unknown,
        Account,
        Card,
        Charge,
        Coupon,
        Customer,
        Discount,
        Dispute,
        Event,
        InvoiceItem,
        Invoice,
        LineItem,
        Plan,
        Subscription,
        Token,
        Transfer,
    }
}
