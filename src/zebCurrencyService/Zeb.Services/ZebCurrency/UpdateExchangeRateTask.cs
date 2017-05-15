using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zeb.Data.Common;
using Zeb.Services.Task;

namespace Zeb.Services.ZebCurrency
{
    /// <summary>
    /// Represents a task for updating exchange rates
    /// </summary>
    public partial class UpdateExchangeRateTask : ITask
    {
        private readonly ICurrencyService _currencyService;
        
        public UpdateExchangeRateTask(ICurrencyService currencyService)
        {
            this._currencyService = currencyService;
        }


        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            

            var primaryCurrencyCode = _currencyService.GetCurrencyById(ZebCurrencySettings.PrimaryExchangeRateCurrencyId).CurrencyCode;
            var exchangeRates = _currencyService.GetCurrencyLiveRates(primaryCurrencyCode);

            foreach (var exchageRate in exchangeRates)
            {
                var currency = _currencyService.GetCurrencyByCode(exchageRate.CurrencyCode);
                if (currency != null)
                {
                    currency.Rate = exchageRate.Rate;
                    currency.TimeStamp = exchageRate.TimeStamp;
                    currency.UpdatedOnUtc = DateTime.UtcNow;
                    _currencyService.UpdateCurrency(currency);
                }
            }


        }
    }
}
