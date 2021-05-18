using Microsoft.Extensions.Configuration;
using Moq;
using PackageControl.Core.Package.Commands;
using PackageControl.Data.Repository;
using PackageControl.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests.Repository
{
    public class PackageRepositoryTest
    {
        private Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();

        private DbSession session;

        public PackageRepositoryTest()
        {
            mockConfig.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value).Returns("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PackageControl;Data Source=DESKTOP-123456");

            session = new DbSession(mockConfig.Object);
        }

        [Fact(Skip = "DataBase test")]
        public async Task UpdatePackagesAsync_Must_Update_Package()
        {
            var dao = new PackageRepository(session);

            var command = new UpdatePackageCommand
            {
                ActualStatus = 1,
                City = "CityPackageRepositoryTest",
                Country = "CountryPackageRepositoryTest",
                HasValueToPay = false,
                IsFragile = true,
                IsStoppedInCustoms = false,
                PackageWeight = 100,
                PlaceType = 2,
                Price = 200,
                Size = 1,
                TrackingCode = "teste-teste",
                TypeOfControl = 0,
                AreaToDeliver = "AreaToDeliverPackageRepositoryTest"
            };

            await dao.UpdatePackagesAsync(command, 1);
        }

        [Fact(Skip = "DataBase test")]
        public async Task HasPackageByTrackingCodeAsync_Verify_If_Has_Package()
        {
            var dao = new PackageRepository(session);

            var result = await dao.HasPackageByTrackingCodeAsync("teste-teste");

            Assert.True(result);
        }

        [Fact(Skip = "DataBase test")]
        public async Task InsertPackagesAsync_Must_Insert_Package()
        {
            var dao = new PackageRepository(session);

            var command = new InsertPackageCommand
            {
                ActualStatus = 1,
                City = "CityPackageRepositoryTest",
                Country = "CountryPackageRepositoryTest",
                HasValueToPay = false,
                IsFragile = true,
                IsStoppedInCustoms = false,
                PackageWeight = 100,
                PlaceType = 2,
                Price = 200,
                Size = 1,
                TypeOfControl = 0,
                AreaToDeliver = "AreaToDeliver1"
            };

            command.SetReceivedDate(DateTime.Now);

            await dao.InsertPackagesAsync("PackageRepositoryTest-PackageRepositoryTest", command, 1);
        }
    }
}
