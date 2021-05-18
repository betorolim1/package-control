using PackageControl.Core.Package.Commands;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Core.Package.Repository
{
    public interface IPackagesRepository
    {
        Task UpdatePackagesAsync(UpdatePackageCommand package, long lastCheckpointId);
        Task<bool> HasPackageByTrackingCode(string trackingCode);
        Task InsertPackagesAsync(string trackingCode, DateTime receivedDate, InsertPackageCommand package, long lastCheckpointId);
    }
}
