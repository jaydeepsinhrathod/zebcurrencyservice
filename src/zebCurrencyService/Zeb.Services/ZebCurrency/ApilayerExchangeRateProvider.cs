using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeb.Core.Domain;

namespace Zeb.Services.ZebCurrency
{
    /// <summary>
    /// Exchange rate provider by apiLayer
    /// </summary>
    public partial class ApilayerExchangeRateProvider : IExchangeRateProvider
    {
        private readonly string _apiAccessKey;
        public ApilayerExchangeRateProvider()
        {
            _apiAccessKey = "0f6e2a2b4dc19e5cdee2d09d3103c09f";
        }

        //this class is only useful here only this so better to place here
        private class ApiResponse
        {
            public string source { get; set; }

           
            public int Timestamp { get; set; }

            public Dictionary<string,decimal> quotes  { get; set; }


        }
     

        //Here I not implemented async method as our task is already running on separate thread 
        public  IList<ExchangeRate> GetCurrencyLiveRates(string primaryCurrencyCode)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            //creating api call
            var client = new RestClient("http://apilayer.net/api");
            ApiResponse resp = new ApiResponse();
            var request = new RestRequest("/live?access_key={access_key}&source={source}", Method.GET);
            request.AddQueryParameter("access_key", _apiAccessKey);
            request.AddQueryParameter("source", primaryCurrencyCode);


            var response = client.Execute<ApiResponse>(request);
            resp = response.Data;

            //iterate through list of currency rate from exchange
            foreach (var quote in resp.quotes)
            {
                var eRate = new ExchangeRate();
                eRate.CurrencyCode = quote.Key.Remove(0,3);
                eRate.UpdatedOn = DateTime.UtcNow;
                eRate.Rate = quote.Value;
                eRate.TimeStamp = resp.Timestamp;
                exchangeRates.Add(eRate);
            }

            return exchangeRates;
        }
    }
}
