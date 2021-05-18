using PackageControl.Core.Country;
using PackageControl.Core.Country.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PackageControl.Data.Service
{
    public class CountryService : ICountryService
    {
        private const string URL = "https://restcountries.eu/rest/v2/name/{0}?fullText=true&fields=alpha2Code;numericCode";

        private readonly IHttpClientFactory _clientFactory;

        public CountryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<CountryDto> GetCountryData(string country)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, String.Format(URL, country));

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content
                    .ReadAsAsync<List<CountryDto>>();

                if (result.Count > 1)
                    throw new Exception("Country name must be exact or correct");

                return result.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
