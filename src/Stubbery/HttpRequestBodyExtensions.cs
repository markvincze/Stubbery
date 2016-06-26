using Microsoft.AspNetCore.Http;
using System.IO;

namespace Stubbery
{
    public static class HttpRequestBodyExtensions
    {
        public static Stream GetCopyOfBodyStream(this HttpRequest httpRequest)
        {
            var ms1 = new MemoryStream();
            httpRequest.Body.CopyTo(ms1);

            ms1.Position = 0;

            var ms2 = new MemoryStream();
            ms1.CopyTo(ms2);

            ms1.Position = 0;
            ms2.Position = 0;

            httpRequest.Body = ms1;

            return ms2;
        }
    }
}