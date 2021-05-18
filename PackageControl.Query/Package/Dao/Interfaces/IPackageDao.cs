using PackageControl.Query.Package.Dao.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Query.Package.Dao.Interfaces
{
    public interface IPackageDao
    {
        Task<List<PackageDto>> GetPackageByTrackingCodesAsync(string[] trackingCodes);
        Task<List<string>> GetTrackingCodesByStatusAsync(byte statusNumber);
        Task<List<string>> GetTrackingCodesByPlaceTypeAsync(byte placeType);
        Task<decimal> GetStatusAmountMoneyAsync(byte statusNumber);
    }
}
