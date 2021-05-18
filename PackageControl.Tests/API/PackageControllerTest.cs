using Microsoft.AspNetCore.Mvc;
using Moq;
using package_control.Controllers;
using PackageControl.Core.Handlers.Interfaces;
using PackageControl.Core.Package.Commands;
using PackageControl.Query.Handlers.Interface;
using PackageControl.Query.Package.Result;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests.API
{
    public class PackageControllerTest
    {
        private Mock<IPackageQueryHandler> mockPackageQueryHandler = new Mock<IPackageQueryHandler>();
        private Mock<IPackageHandler> mockPackageHandler = new Mock<IPackageHandler>();

        [Fact]
        public async Task GetPackageByTrackingCodeListAsync_Most_Return_OkObjectResult()
        {
            var trackingCodes = new string[]
            {
                "trackingCodesTest1",
                "trackingCodesTest2"
            };

            var packageResult = new List<PackageResult>()
            {
                new PackageResult
                {
                   TrackingCode = "trackingCodesTest1"
                },
                new PackageResult
                {
                   TrackingCode = "trackingCodesTest2"
                },
            };

            mockPackageQueryHandler.Setup(x => x.GetPackageByTrackingCodesAsync(trackingCodes)).ReturnsAsync(packageResult);

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.GetPackageByTrackingCodeListAsync(trackingCodes) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task GetPackageByTrackingCodeAsync_Most_Return_OkObjectResult()
        {
            var trackingCode = "trackingCodeTest";

            var packageResult = new List<PackageResult>()
            {
                new PackageResult
                {
                   TrackingCode = "trackingCodeTest"
                }
            };

            mockPackageQueryHandler.Setup(x => x.GetPackageByTrackingCodesAsync(new string[] { trackingCode })).ReturnsAsync(packageResult);

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.GetPackageByTrackingCodeAsync(trackingCode) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task GetTrackingCodesByStatusAsync_Most_Return_OkObjectResult()
        {
            var trackingCodeList = new List<string>
            {
                "trackingCodeTest1",
                "trackingCodeTest2"
            };

            mockPackageQueryHandler.Setup(x => x.GetTrackingCodesByStatusAsync(1)).ReturnsAsync(trackingCodeList);

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.GetTrackingCodesByStatusAsync(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task GetTrackingCodesByPlaceTypeAsync_Most_Return_OkObjectResult()
        {
            var trackingCodeList = new List<string>
            {
                "trackingCodeTest1",
                "trackingCodeTest2"
            };

            mockPackageQueryHandler.Setup(x => x.GetTrackingCodesByPlaceTypeAsync(1)).ReturnsAsync(trackingCodeList);

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.GetTrackingCodesByPlaceTypeAsync(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task GetStatusAmountMoneyAsync_Most_Return_OkObjectResult()
        {
            mockPackageQueryHandler.Setup(x => x.GetStatusAmountMoneyAsync(1)).ReturnsAsync(100);

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.GetStatusAmountMoneyAsync(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task UpdatePackagesAsync_Most_Return_OkResult()
        {
            var commandList = new List<UpdatePackageCommand>
            {
                new UpdatePackageCommand()
            };

            mockPackageHandler.Setup(x => x.UpdatePackagesAsync(commandList));

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.UpdatePackagesAsync(commandList) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task UpdatePackagesAsync_Most_Return_BadRequest()
        {
            mockPackageHandler.Setup(x => x.UpdatePackagesAsync(null));

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.UpdatePackagesAsync(null) as BadRequestResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task InsertPackagesAsync_Most_Return_OkObjectResult()
        {
            var commandList = new List<InsertPackageCommand>
            {
                new InsertPackageCommand()
            };

            mockPackageHandler.Setup(x => x.InsertPackagesAsync(commandList));

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.InsertPackagesAsync(commandList) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }

        [Fact]
        public async Task InsertPackagesAsync_Most_Return_BadRequest()
        {
            mockPackageHandler.Setup(x => x.InsertPackagesAsync(null));

            var controller = new PackagesController(mockPackageQueryHandler.Object, mockPackageHandler.Object);

            var result = await controller.InsertPackagesAsync(null) as BadRequestResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);

            mockPackageQueryHandler.VerifyAll();
        }
    }
}
