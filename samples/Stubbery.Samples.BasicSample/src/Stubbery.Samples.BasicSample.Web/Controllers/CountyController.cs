using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stubbery.Samples.BasicSample.Web.Controllers
{
    [Route("")]
    public class CountyController : Controller
    {
        private readonly IConfiguration configuration;

        public CountyController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Route("countyname/{postcode}")]
        public async Task<IActionResult> GetCountyName(string postcode)
        {
            using (var hc = new HttpClient())
            {
                var uri = new UriBuilder(configuration["PostCodeApiUrl"])
                {
                    Path = $"postcodes/{postcode}"
                }.Uri;

                var response = await hc.GetAsync(uri);

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(reader))
                {
                    var jObject = JObject.Load(jsonTextReader);

                    if (jObject["status"].Value<string>() == "200")
                    {
                        return Ok(jObject["result"]["admin_county"].Value<string>());
                    }

                    if (jObject["status"].Value<string>() == "404")
                    {
                        return NotFound();
                    }

                    return StatusCode(500);
                }
            }
        }
    }
}