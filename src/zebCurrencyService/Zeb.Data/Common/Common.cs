using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeb.Data.Common
{
    public class Common
    {
    }

    //IN PRODUCTION THIS SETTINGS WILL COME FROM DATABASE
    public static class ZebCurrencySettings
    {


        public static int PrimaryExchangeRateCurrencyId
        {
            //INR as a primary 
            get { return 1; }
        }
        public static int PrimaryZebApiTargetCurrencyId
        {
            //INR primary target currency for zeb api
            get { return 1; }
        }

        public static string DefaultRateReqCurrencyCode
        {
            get { return "USD"; }
        }
    }


    public static class ApiMessages
    {
        public static string SuccessMessage { get { return "success"; } }

        public static int SuccessCode { get { return 1; } }

    }
}
