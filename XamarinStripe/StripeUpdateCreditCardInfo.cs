using System;
using System.Text;
using System.Web;

namespace Xamarin.Payments.Stripe
{
    public class StripeUpdateCreditCardInfo : StripeId, IUrlEncoderInfo
    {
        // All fields optional when performing an update
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public virtual void UrlEncode (StringBuilder sb)
        {
            if (ExpirationMonth.HasValue && (ExpirationMonth <= 0 || ExpirationMonth > 12))
                throw new ArgumentOutOfRangeException ("ExpirationMonth");
            if (ExpirationYear.HasValue && ExpirationYear <= 0)
                throw new ArgumentOutOfRangeException ("ExpirationYear");

            if (ExpirationMonth.HasValue) 
                sb.AppendFormat ("exp_month={0}&", HttpUtility.UrlEncode (ExpirationMonth.Value.ToString ()));
            if (ExpirationYear.HasValue)
                sb.AppendFormat ("exp_year={0}&", HttpUtility.UrlEncode (ExpirationYear.Value.ToString ()));
            if (!String.IsNullOrEmpty (FullName))
                sb.AppendFormat ("name={0}&", HttpUtility.UrlEncode (FullName));
            if (!String.IsNullOrEmpty (AddressLine1))
                sb.AppendFormat ("address_line1={0}&", HttpUtility.UrlEncode (AddressLine1));
            if (!String.IsNullOrEmpty (AddressLine2))
                sb.AppendFormat ("address_line2={0}&", HttpUtility.UrlEncode (AddressLine2));
            if (!String.IsNullOrEmpty (ZipCode))
                sb.AppendFormat ("address_zip={0}&", HttpUtility.UrlEncode (ZipCode));
            if (!String.IsNullOrEmpty (StateOrProvince))
                sb.AppendFormat ("address_state={0}&", HttpUtility.UrlEncode (StateOrProvince));
            if (!String.IsNullOrEmpty (Country))
                sb.AppendFormat ("address_country={0}&", HttpUtility.UrlEncode (Country));
            if (!String.IsNullOrEmpty (City))
                sb.AppendFormat ("address_city={0}&", HttpUtility.UrlEncode (City));

            if (sb.Length == 0)
                throw new InvalidOperationException ("No card information provided");
        }
    }
}
