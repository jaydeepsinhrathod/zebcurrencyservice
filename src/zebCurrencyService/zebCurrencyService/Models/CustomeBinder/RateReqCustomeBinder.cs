using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace zebCurrencyService.Models.CustomeBinder
{
    /// <summary>
    /// Custome model binding for rate request with default value
    /// </summary>
    public class RateReqCustomeBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }
}