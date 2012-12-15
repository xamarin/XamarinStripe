using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {
    /* duplicate, fraudulent, subscription_canceled, product_unacceptable, product_not_received, unrecognized, credit_not_processed, general */
    [JsonConverter (typeof (StripeEnumConverter<StripeDisputeReason>))]
    public enum StripeDisputeReason {
        Duplicate,
        Fraudulent,
        SubscriptionCanceled,
        ProductUnacceptable,
        ProductNotReceived,
        Unrecognized,
        CreditNotProcessed,
        General
    }
}
