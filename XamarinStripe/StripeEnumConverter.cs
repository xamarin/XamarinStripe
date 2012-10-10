/*
 * Copyright 2012 Joe Dluzen, 2012 Xamarin, Inc.
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Xamarin.Payments.Stripe {
    public class StripeEnumConverter<T> : JsonConverter where T : struct, IConvertible {
        Dictionary<string, string> values;
        public StripeEnumConverter ()
        {
            values = new Dictionary<string, string> ();
            if (!typeof (T).IsEnum)
                throw new InvalidCastException ("Specified type T must be an enum");
        }

        public override bool CanConvert (Type objectType)
        {
            return objectType == typeof (T);
        }

        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            var name = reader.Value as string;
            name = name.Replace ("_", "");
            
            return Enum.Parse (typeof (T), name, true);
        }

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            string key = value.ToString ();
            string result;

            if (!values.TryGetValue (key, out result)) {
                result = Regex.Replace (key, @"(?<!^|_|[A-Z])([A-Z])", "_$1").ToLowerInvariant ();
                values [key] = result;
            }
            writer.WriteValue (result);
        }
    }
}
