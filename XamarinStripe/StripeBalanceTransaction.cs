using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Xamarin.Payments.Stripe {

    [JsonConverter (typeof (StripeEnumConverter<StripeBalanceTransactionType>))]
    public enum StripeBalanceTransactionType {
        Charge,
        Refund,
        Adjustment,
        ApplicationFee,
        ApplicationFeeRefund,
        Transfer,
        TransferCancel,
        TransferFailure
    }

    public class StripeBalanceTransaction : StripeObject {
        [JsonProperty ("source")]
        public string Source { get; set; }
        
        [JsonProperty ("amount")]
        public int Amount { get; set; }

        [JsonProperty ("currency")]
        public string Currency { get; set; }

        [JsonProperty ("available_on")]
        public int Net { get; set; }

        [JsonProperty ("fee")]
        public int Fee { get; set; }

        [JsonProperty ("description")]
        public string Description { get; set; }

        [JsonProperty ("status")]
        public string Status { get; set; }

        [JsonProperty ("type")]
        public StripeBalanceTransactionType Type { get; set; }
        
        [JsonProperty ("available_on")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime AvailableOn { get; set; }
        
        [JsonProperty ("created")]
        [JsonConverter (typeof (UnixDateTimeConverter))]
        public DateTime Created { get; set; }
    }
}
