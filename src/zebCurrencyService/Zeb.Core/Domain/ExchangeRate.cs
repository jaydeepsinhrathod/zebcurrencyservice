﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeb.Core.Domain
{
    /// <summary>
    /// Represents an exchange rate
    /// </summary>
    public partial class ExchangeRate
    {
        /// <summary>
        /// Creates a new instance of the ExchangeRate class
        /// </summary>
        public ExchangeRate()
        {
            CurrencyCode = string.Empty;
            Rate = 1.0m;
        }

        /// <summary>
        /// The three letter ISO code for the Exchange Rate, e.g. USD
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// The conversion rate of this currency from the base currency
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// When was this exchange rate updated from the data source (the internet data xml feed) : temp we are using over system utc
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        public int TimeStamp { get; set; }


        /// <summary>
        /// Format the rate into a string with the currency code, e.g. "USD 0.72543"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", this.CurrencyCode, this.Rate);
        }
    }
}
