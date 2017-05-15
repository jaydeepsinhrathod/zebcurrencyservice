using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Zeb.Data.Common;

namespace zebCurrencyService.Models.Rate
{
    public class RateRequest
    {
        //seting a default value for req
        public RateRequest()
        {
            Amount = 1.000m;
            CurrencyCode = ZebCurrencySettings.DefaultRateReqCurrencyCode;
        }

        /// <summary>
        /// Valid ISO currency code to convert from INR. Default:USD (If Not Specified)
        /// </summary>
        [Required]
        public string CurrencyCode { get; set; }


        /// <summary>
        /// Valid decimal Amount to convert from INR to Specified CurrencyCode Default:1 (If Not Specified)
        /// </summary>
        [Required]
        public decimal Amount { get; set; }
    }
}