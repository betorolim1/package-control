using Microsoft.Extensions.Configuration;
using Moq;
using PackageControl.Core.Handlers;
using PackageControl.Core.LastCheckpoint;
using PackageControl.Core.Package.Commands;
using PackageControl.Core.Package.Repository;
using PackageControl.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests
{
    public class PackageHandlerTest
    {
        private Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        private Mock<IPackagesRepository> mockPackagesRepository = new Mock<IPackagesRepository>();
        private Mock<ILastCheckpointRepository> mockLastCheckpointRepository = new Mock<ILastCheckpointRepository>();
        private Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        public PackageHandlerTest()
        {
            mockConfiguration.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value).Returns("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PackageControl;Data Source=DESKTOP-123456");
        }

        [Fact]
        public async Task Must_Get_Does_Not_Exists_Error()
        {
            var command = new List<UpdatePackageCommand>
            {
                getUpdatePackage()
            };

            mockPackagesRepository.Setup(x => x.HasPackageByTrackingCode("TEST-TEST")).ReturnsAsync(false);

            var handler = new PackageHandler(mockPackagesRepository.Object, mockLastCheckpointRepository.Object, mockUnitOfWork.Object);

            await handler.UpdatePackagesAsync(command);
        }

        private UpdatePackageCommand getUpdatePackage()
        {
            return new UpdatePackageCommand
            {
                ActualStatus = 1,
                City = "ABC",
                Country = "DEF",
                HasValueToPay = false,
                IsFragile = true,
                IsStoppedInCustoms = false,
                PackageWeight = 100,
                PlaceType = 2,
                Price = 200,
                Size = 1,
                TrackingCode = "TEST-TEST",
                TypeOfControl = 0
            };
        }
    }
}
