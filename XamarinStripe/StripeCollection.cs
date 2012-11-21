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
    public class StripeCollection<T> : IEnumerable<T> {
        [JsonProperty ("url")]
        public string Url { get; set; }

        [JsonProperty ("count")]
        public int Total { get; set; }

        [JsonProperty ("data")]
        public List<T> Data { get; set; }

        #region IEnumerable[T] implementation
        public IEnumerator<T> GetEnumerator ()
        {
            return Data.GetEnumerator ();
        }
        #endregion

        #region IEnumerable implementation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator ();
        }
        #endregion
    }
}
