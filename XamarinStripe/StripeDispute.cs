using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    public class StripeDispute : StripeObject {
        [JsonProperty("status")]
        public StripeDisputeStatus Status { get; set; }

        [JsonProperty ("evidence")]
        public string Evidence { get; set; }

        [JsonProperty ("charge")]
        public string Charge { get; set; }

        [JsonProperty ("created")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime? Created { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("amount")]
        public int Amount;

        [JsonProperty ("livemode")]
        public bool LiveMode { get; set; }

        [JsonProperty ("reason")]
        public StripeDisputeReason Reason { get; set; }

        [JsonProperty ("evidence_due_by")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime? EvidenceDueBy { get; set; }
    }
}
