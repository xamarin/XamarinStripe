/*
 * Copyright 2012 Joe Dluzen
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
    public class SubscriptionStatusConverter : JsonConverter {
        protected const string PastDue = "past_due";

        public override bool CanConvert (Type objectType)
        {
            return objectType == typeof(StripeSubscriptionStatus);
        }

        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw new Exception (string.Format ("Unexpected token parsing StripeSubscriptionStatus. Expected String, got {0}.", reader.TokenType));

            string value = reader.Value as string;
            if (value == PastDue)
                return StripeSubscriptionStatus.PastDue;
            return Enum.Parse (typeof(StripeSubscriptionStatus), value);
        }

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            if ((StripeSubscriptionStatus) value == StripeSubscriptionStatus.PastDue)
                writer.WriteValue (PastDue);
            else
                writer.WriteValue (value.ToString ());
        }
    }
}
