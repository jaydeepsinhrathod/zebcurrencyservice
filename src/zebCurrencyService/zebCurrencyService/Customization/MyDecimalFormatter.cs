using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace zebCurrencyService.Customization
{
    public class MyDecimalFormatter : MediaTypeFormatter
    {
        public MyDecimalFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(decimal);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            var decimalValue = (decimal)value;

            var formatted = decimalValue.ToString("F2", CultureInfo.InvariantCulture);

            using (var writer = new StreamWriter(writeStream))
            {
                writer.Write(formatted);
            }

            return Task.FromResult(0);
        }
    }
}