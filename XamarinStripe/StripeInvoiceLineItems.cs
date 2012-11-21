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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripeInvoiceLineItems : IEnumerable<StripeLineItem> {
        [JsonProperty (PropertyName = "invoiceitems")]
        public List<StripeLineItem> InvoiceItems { get; set; }

        [JsonProperty (PropertyName = "prorations")]
        public List<StripeLineItem> Prorations { get; set; }

        [JsonProperty (PropertyName = "subscriptions")]
        public List<StripeLineItem> Subscriptions { get; set; }

        #region IEnumerable[StripeLineItem] implementation
        public IEnumerator<StripeLineItem> GetEnumerator ()
        {
            return ((IEnumerable<StripeLineItem>)InvoiceItems).Concat (Prorations).Concat (Subscriptions).GetEnumerator();
        }
        #endregion

        #region IEnumerable implementation
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }
        #endregion

    }
}
