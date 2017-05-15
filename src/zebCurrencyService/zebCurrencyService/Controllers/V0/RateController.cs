using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Zeb.Core.Infrastructure;
using Zeb.Data.Common;
using Zeb.Services.Task;
using Zeb.Services.ZebCurrency;
using zebCurrencyService.ExceptionFilters.V0;
using zebCurrencyService.Models;
using zebCurrencyService.Models.Rate;

namespace zebCurrencyService.Controllers.V0
{
    [ApiVersion("0")]
    [Route("api/v{version:apiVersion}/rate")]
    public class RateController : ApiController
    {
        #region Fields
        //private readonly IScheduleTaskService _scheduleTaskService;

        //#endregion

        //#region Constructors
        //public RateController(IScheduleTaskService scheduleTaskService)
        //{
        //    this._scheduleTaskService = scheduleTaskService;

        //}

        #endregion




        /// <summary>
        /// Get other currency rate of INR
        /// </summary>
        /// <param name="rateReq">rateReq</param>
        /// <returns></returns>

        [HttpPost]
        [RateExceptionFilter]
        public IHttpActionResult Post([FromBody]RateRequest rateReq)
        {

            RateResponse response = new RateResponse();
            //use this link to compare api result  https://currency-api.appspot.com/
            if (!ModelState.IsValid)
                throw new Exception("Parameter validation failed");

            //this will going to replace when i will do custome model binder to assign default value
            if (rateReq == null)
                rateReq = new RateRequest { Amount = 1, CurrencyCode = "USD" }; //default if null

            //TODO : Dependency Injection for web api As it Is Important for Test Driven Environment
            //temporary I resolve with AutoFac Container
            var currencyService = EngineContext.Current.ContainerManager.Resolve<ICurrencyService>();
            var sourceCurrency = currencyService.GetCurrencyByCode(rateReq.CurrencyCode);


            //calcultaing rate for given amount
            var rate = currencyService.ConvertToPrimaryExchangeRateCurrency(rateReq.Amount, sourceCurrency);



            //creating valid rate conversion response
            response.ConversionRate = Math.Round((1.000000m / sourceCurrency.Rate), 2);
            response.Total = Math.Round(rate, 2);
            response.returncode = ApiMessages.SuccessCode;
            response.SourceCurrency = sourceCurrency.CurrencyCode;
            response.err = ApiMessages.SuccessMessage;
            response.TimeStamp = (int)sourceCurrency.TimeStamp;
            response.Amount = rateReq.Amount;

            return Ok(response);




        }
    }
}
