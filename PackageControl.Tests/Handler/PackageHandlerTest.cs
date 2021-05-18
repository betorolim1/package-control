using Microsoft.Extensions.Configuration;
using Moq;
using PackageControl.Core.Country;
using PackageControl.Core.Country.Service;
using PackageControl.Core.Handlers;
using PackageControl.Core.LastCheckpoint;
using PackageControl.Core.Package.Commands;
using PackageControl.Core.Package.Repository;
using PackageControl.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests.Handler
{
    public class PackageHandlerTest
    {
        private Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        private Mock<IPackagesRepository> mockPackagesRepository = new Mock<IPackagesRepository>();
        private Mock<ILastCheckpointRepository> mockLastCheckpointRepository = new Mock<ILastCheckpointRepository>();
        private Mock<ICountryService> mockCountryService = new Mock<ICountryService>();
        private Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();

        public PackageHandlerTest()
        {
            mockConfiguration.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value).Returns("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PackageControl;Data Source=DESKTOP-123456");

            mockUnitOfWork.Setup(x => x.BeginTransactionAsync());
        }

        [Fact]
        public async Task InsertPackagesAsync_Must_Insert_Package()
        {
            var correctTrackingCode = "PT-1revile-310899-250321-59";

            var command = new List<InsertPackageCommand>
            {
                getInsertPackage()
            };

            mockCountryService.Setup(x => x.GetCountryData("Portugal")).ReturnsAsync(new CountryDto { Alpha2Code = "PT", NumericCode = "620" });

            mockLastCheckpointRepository.Setup(x => x.InsertLastCheckpoint("Portugal", "City1", 0, 2)).ReturnsAsync(10);

            mockPackagesRepository.Setup(x => x.InsertPackagesAsync(It.IsAny<string>(), command.FirstOrDefault(), 10));

            var handler = new PackageHandler(mockPackagesRepository.Object, mockLastCheckpointRepository.Object, mockUnitOfWork.Object, mockCountryService.Object);

            var result = await handler.InsertPackagesAsync(command);

            Assert.NotNull(result);
            Assert.Single(result);

            Assert.Equal(correctTrackingCode.Substring(0, 11), result[0].Substring(0, 11));
            Assert.Equal(correctTrackingCode.Substring(17, 10), result[0].Substring(17, 10));

            mockCountryService.VerifyAll();
            mockLastCheckpointRepository.VerifyAll();
            mockPackagesRepository.VerifyAll();
            mockUnitOfWork.VerifyAll();
        }

        [Fact]
        public async Task InsertPackagesAsync_Must_Return_Country_Not_Found()
        {
            var command = new List<InsertPackageCommand>
            {
                new InsertPackageCommand
                {
                    Country = "COUNTRYTEST"
                }
            };

            mockUnitOfWork.Setup(x => x.Rollback());

            mockCountryService.Setup(x => x.GetCountryData("COUNTRYTEST")).ReturnsAsync(new CountryDto());

            var handler = new PackageHandler(mockPackagesRepository.Object, mockLastCheckpointRepository.Object, mockUnitOfWork.Object, mockCountryService.Object);

            var exception = await Record.ExceptionAsync(() => handler.InsertPackagesAsync(command));

            Assert.NotNull(exception);
            Assert.Equal("Country COUNTRYTEST not found", exception.Message);
            Assert.IsType<Exception>(exception);

            mockCountryService.VerifyAll();
            mockUnitOfWork.VerifyAll();
        }

        [Fact]
        public async Task UpdatePackagesAsync_Must_Update_Package()
        {
            var command = new List<UpdatePackageCommand>
            {
                getUpdatePackage()
            };

            mockPackagesRepository.Setup(x => x.HasPackageByTrackingCodeAsync("TEST-TEST")).ReturnsAsync(true);

            mockLastCheckpointRepository.Setup(x => x.InsertLastCheckpoint("Portugal", "City1", 0, 2)).ReturnsAsync(10);

            mockPackagesRepository.Setup(x => x.UpdatePackagesAsync(command.FirstOrDefault(), 10));

            var handler = new PackageHandler(mockPackagesRepository.Object, mockLastCheckpointRepository.Object, mockUnitOfWork.Object, mockCountryService.Object);

            await handler.UpdatePackagesAsync(command);

            mockUnitOfWork.VerifyAll();
            mockLastCheckpointRepository.VerifyAll();
            mockPackagesRepository.VerifyAll();
        }

        [Fact]
        public async Task UpdatePackagesAsync_Must_Validade_When_TrackingCode_Does_Not_Exist()
        {
            var command = new List<UpdatePackageCommand>
            {
                getUpdatePackage()
            };

            mockPackagesRepository.Setup(x => x.HasPackageByTrackingCodeAsync("TEST-TEST")).ReturnsAsync(false);

            var handler = new PackageHandler(mockPackagesRepository.Object, mockLastCheckpointRepository.Object, mockUnitOfWork.Object, mockCountryService.Object);

            var exception = await Record.ExceptionAsync(() => handler.UpdatePackagesAsync(command));

            Assert.NotNull(exception);
            Assert.Equal("TEST-TEST does not exists", exception.Message);
            Assert.IsType<Exception>(exception);

            mockUnitOfWork.VerifyAll();
            mockPackagesRepository.VerifyAll();
        }

        private UpdatePackageCommand getUpdatePackage()
        {
            return new UpdatePackageCommand
            {
                ActualStatus = 1,
                City = "City1",
                Country = "Portugal",
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

        private InsertPackageCommand getInsertPackage()
        {
            var command = new InsertPackageCommand
            {
                ActualStatus = 1,
                City = "City1",
                Country = "Portugal",
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

            command.SetReceivedDate(new DateTime(2021, 03, 25));

            return command;
        }
    }
}
