using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    /* won, lost, needs_response, under_review */
    [JsonConverter (typeof (StripeEnumConverter<StripeDisputeStatus>))]
    public enum StripeDisputeStatus {
        Won,
        Lost,
        NeedsResponse,
        UnderReview
    }
}
