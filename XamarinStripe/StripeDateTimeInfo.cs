using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Xamarin.Payments.Stripe {
    public class StripeDateTimeInfo : IUrlEncoderInfo {
        public DateTime gt { set { values["gt"] = value; } }
        public DateTime gte { set { values["gte"] = value; } }
        public DateTime lt { set { values["lt"] = value; } }
        public DateTime lte { set { values["lte"] = value; } }
        public DateTime on { set { values ["on"] = value; } }

        Dictionary<string,DateTime> values = new Dictionary<String,DateTime> ();
        internal string Prefix { get; set; }

        public virtual void UrlEncode (StringBuilder sb)
        {
            if (values.ContainsKey ("on")) {
                sb.AppendFormat ("{0}={1}&", Prefix, values ["on"].ToUnixEpoch ());
            } else {
                foreach (var key in values.Keys) {
                    var date = values [key];
                    sb.AppendFormat ("{0}[{1}]={2}&", Prefix, HttpUtility.UrlEncode (key), date.ToUnixEpoch ());
                }
            }
        }
    }
}
