using PackageControl.Query.Package.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageControl.Query.Handlers.Interface
{
    public interface IPackageQueryHandler
    {
        Task<List<PackageResult>> GetPackageByTrackingCodesAsync(string[] trackingCodes);
        Task<List<string>> GetTrackingCodesByStatusAsync(byte statusNumber);
        Task<List<string>> GetTrackingCodesByPlaceTypeAsync(byte placeType);
        Task<decimal> GetStatusAmountMoneyAsync(byte statusNumber);
    }
}
