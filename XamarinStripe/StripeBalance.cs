using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    public class StripeBalance :StripeObject {       
        [JsonProperty ("pending")]
        public List<Value> Pending { get; set; }
        [JsonProperty ("available")]
        public List<Value> Available { get; set; }

        public class Value {
            [JsonProperty ("amount")]
            public int Amount { get; set; }

            [JsonProperty ("currency")]
            public string Currency { get; set; }
        }
    }
}
