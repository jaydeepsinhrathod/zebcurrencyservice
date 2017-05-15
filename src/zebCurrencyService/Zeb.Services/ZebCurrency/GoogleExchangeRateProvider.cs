using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zeb.Core.Domain;
using Zeb.Data;

namespace Zeb.Services.ZebCurrency
{
    /// <summary>
    /// Exchange rate provider by google
    /// </summary>
    public partial class GoogleExchangeRateProvider : IExchangeRateProvider
    {


        public IList<ExchangeRate> GetCurrencyLiveRates(string primaryCurrencyCode, List<Currency> supportedCurrencies)
        {
                         
            //here I am not making code async because it will going to run as seperate thread through task

            List<ExchangeRate> rates = new List<ExchangeRate>();

            foreach (var currency in supportedCurrencies.Where(S => S.CurrencyCode != primaryCurrencyCode))
            {
                WebClient web = new WebClient();

                string url = string.Format("https://www.google.com/finance/converter?a={2}&from={0}&to={1}", primaryCurrencyCode,
                   currency.CurrencyCode, 1.00);

               
                string response = web.DownloadString(url);

                var split = response.Split((new string[] { "<span class=bld>" }), StringSplitOptions.None);
                var value = split[1].Split(' ')[0];
                decimal rate = decimal.Parse(value, CultureInfo.InvariantCulture);

                rates.Add(new ExchangeRate
                {
                    CurrencyCode = currency.CurrencyCode,

                    Rate = rate,
                    UpdatedOn = DateTime.UtcNow
                });

            }

            //add primary currency exchange rate as 1.00
            rates.Add(new ExchangeRate
            {
                CurrencyCode = primaryCurrencyCode,
                Rate = 1.000m,
                UpdatedOn = DateTime.UtcNow

            });

            return rates;
        }
    }
}
