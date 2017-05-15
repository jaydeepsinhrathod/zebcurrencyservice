using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using zebCurrencyService.Models.Rate;

namespace zebCurrencyService.ExceptionFilters.V0
{
    /// <summary>
    /// Exception filter for rate controller
    /// </summary>
    public class RateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var request = context.ActionContext.Request;

            var response = new  RateResponse
            {
                returncode = 500, //temp assigning error code need to change 1 use for success
                err= context.Exception.Message //just sending message,details exception message may require other property 
                 
            };

            context.Response = request.CreateResponse(HttpStatusCode.BadRequest, response);
        }
    }
}