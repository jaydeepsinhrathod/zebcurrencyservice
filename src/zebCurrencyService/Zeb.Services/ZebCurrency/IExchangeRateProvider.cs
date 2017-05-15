using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeb.Core.Domain;
using Zeb.Data;

namespace Zeb.Services.ZebCurrency
{
    /// <summary>
    /// Exchange rate provider interface
    /// </summary>
    public partial interface IExchangeRateProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="primaryCurrencyCode"></param>
        /// <returns></returns>
        IList<ExchangeRate> GetCurrencyLiveRates(string primaryCurrencyCode);
    }
}
