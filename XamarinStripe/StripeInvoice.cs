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
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    [JsonObject (MemberSerialization.OptIn)]
    public class StripeInvoice : StripeObject, IEnumerable<StripeInvoiceLineItem> {
        [JsonProperty (PropertyName = "created")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonProperty (PropertyName = "subtotal")]
        public int Subtotal { get; set; }

        [JsonProperty (PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty (PropertyName = "lines")]
        public StripeInvoiceLineItems LineItems { get; set; }

        [JsonProperty (PropertyName = "attempted")]
        public bool Attempted { get; set; }

        [JsonProperty (PropertyName = "closed")]
        public bool Closed { get; set; }

        [JsonProperty (PropertyName = "customer")]
        public string CustomerID { get; set; }

        [JsonProperty (PropertyName = "date")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty (PropertyName = "paid")]
        public bool Paid { get; set; }

        [JsonProperty (PropertyName = "period_start")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime Start { get; set; }

        [JsonProperty (PropertyName = "period_end")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime End { get; set; }

        #region IEnumerable[StripeInvoiceLineItem] implementation
        public IEnumerator<StripeInvoiceLineItem> GetEnumerator ()
        {
            return LineItems.GetEnumerator ();
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
