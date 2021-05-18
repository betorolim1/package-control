using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Core.Country.Service
{
    public interface ICountryService
    {
        Task<CountryDto> GetCountryData(string country);
    }
}
