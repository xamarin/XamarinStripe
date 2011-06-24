/*
 * Copyright 2011 Xamarin, Inc.
 *
 * Author(s):
 * 	Gonzalo Paniagua Javier (gonzalo@xamarin.com)
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
    public class UnixDateTimeConverter : DateTimeConverterBase {
        static bool IsNullable (Type type)
        {
            if (!type.IsValueType)
                return true; // ref-type
            if (Nullable.GetUnderlyingType (type) != null)
                return true; // Nullable<T>
            return false; // value-type
        }

        public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool nullable = IsNullable (objectType);
            Type t = (nullable) ? Nullable.GetUnderlyingType (objectType) : objectType;
            if (reader.TokenType == JsonToken.Null) {
                if (!nullable)
                    throw new Exception (String.Format ("Cannot convert null value to {0}.", objectType));
                return null;
            }

            if (reader.TokenType != JsonToken.Integer)
                throw new Exception (String.Format ("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));

            return ((long) reader.Value).FromUnixEpoch ();
        }

        public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is DateTime))
                throw new Exception ("Invalid value");

            DateTime dt = (DateTime) value;
            writer.WriteValue (dt.ToUnixEpoch ());
        }
    }
}
