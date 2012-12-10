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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    public class StripeObjectConverter : JsonConverter {
        public override bool CanWrite { get { return false; } }

        public override bool CanConvert (Type objectType)
        {
            return typeof (StripeObject).IsAssignableFrom (objectType);
        }

        StripeObject FindType (JObject jobj)
        {
            var type = (string) jobj.Property ("object");
            switch (type) {
            case "account":
                return new StripeAccount ();
            case "charge":
                return new StripeCharge ();
            case "event":
                return new StripeEvent ();
            case "discount":
                return new StripeDiscount ();
            case "dispute":
                return new StripeDispute ();
            case "coupon":
                return new StripeCoupon ();
            case "customer":
                return new StripeCustomer ();
            case "line_item":
                return new StripeLineItem ();
            case "plan":
                return new StripePlan ();
            case "token":
                return new StripeCreditCardToken ();
            case "subscription":
                return new StripeSubscription ();
            case "invoiceitem":
                return new StripeInvoiceItem ();
            case "invoice":
                return new StripeInvoice ();
            case "transfer":
                return new StripeTransfer ();
            }
            return new StripeObject ();
        }

        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load (reader);

            var target = FindType (jobj);

            serializer.Populate (jobj.CreateReader (), target);

            return target;
        }

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = JObject.FromObject (value);
            obj.WriteTo (writer);
        }
    }
}
