using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zebCurrencyService.Models.Rate
{
    /// <summary>
    /// Response data for rate api
    /// </summary>
    public class RateResponse
    {
        /// <summary>
        /// Currency That specified in request for conversion to INR
        /// </summary>
        public string SourceCurrency { get; set; }

        /// <summary>
        /// Conversion Rate of source currency to INR
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// Converted Amount to INR from source currency
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Returns 1 If Success, otherwise Errorcode with error messgae
        /// </summary>
        public int returncode { get; set; }

        /// <summary>
        /// timestamp of exchange rate
        /// </summary>
        public int TimeStamp { get; set; }


        /// <summary>
        /// returns "success" for returncode 1 , and for other error message
        /// </summary>
        public string err { get; set; }

        /// <summary>
        /// Amount pass in request for conversion 
        /// </summary>
        public decimal Amount { get; set; }


    }
}