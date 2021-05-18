using PackageControl.Core.Package.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Core.Handlers.Interfaces
{
    public interface IPackageHandler
    {
        Task UpdatePackagesAsync(List<UpdatePackageCommand> updatePackages);
        Task InsertPackagesAsync(List<InsertPackageCommand> insertPackages);
    }
}
