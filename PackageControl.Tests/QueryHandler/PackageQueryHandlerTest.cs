using Moq;
using PackageControl.Query.Handlers;
using PackageControl.Query.Package.Dao.Dto;
using PackageControl.Query.Package.Dao.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests.QueryHandler
{
    public class PackageQueryHandlerTest
    {
        private Mock<IPackageDao> mockPackageDao = new Mock<IPackageDao>();

        [Fact]
        public async Task GetPackageByTrackingCodesAsync_Most_Return_Packages()
        {
            var trackingCodes = new string[]
            {
                "trackingCodesTest1",
            };

            var dtos = new List<PackageDto>
            {
                getPackageDto()
            };

            mockPackageDao.Setup(x => x.GetPackageByTrackingCodesAsync(trackingCodes)).ReturnsAsync(dtos);

            var handler = new PackageQueryHandler(mockPackageDao.Object);

            var result = await handler.GetPackageByTrackingCodesAsync(trackingCodes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("In Transit", result[0].ActualStatus);
            Assert.Equal("AreaToDeliver1", result[0].AreaToDeliver);
            Assert.False(result[0].HasValueToPay);
            Assert.Equal(2, result[0].Id);
            Assert.True(result[0].IsFragile);
            Assert.False(result[0].IsStoppedInCustoms);
            Assert.NotNull(result[0].LastCheckpoint);
            Assert.Equal("City1", result[0].LastCheckpoint.City);
            Assert.Equal("Country1", result[0].LastCheckpoint.Country);
            Assert.Equal("Port", result[0].LastCheckpoint.PlaceType);
            Assert.Equal("Final Check", result[0].LastCheckpoint.TypeOfControl);
            Assert.Equal(200, result[0].PackageWeight);
            Assert.Equal(400, result[0].Price);
            Assert.Equal(new DateTime(2021, 01, 02), result[0].ReceiveDate);
            Assert.Equal("S", result[0].Size);
            Assert.Equal("TrackingCode1", result[0].TrackingCode);

            mockPackageDao.VerifyAll();
        }

        [Theory]
        [InlineData(0, "Received")]
        [InlineData(1, "In Transit")]
        [InlineData(2, "Stopped By Legal")]
        [InlineData(3, "Attemped Delivery")]
        [InlineData(4, "Returning")]
        [InlineData(5, "Delivered")]
        [InlineData(6, "")]
        public async Task GetPackageByTrackingCodesAsync_Most_Return_Correct_ActualStatus(byte actualStatusKey, string actualStatusValue)
        {
            var trackingCodes = new string[]
            {
                "trackingCodesTest1",
            };

            var dtos = new List<PackageDto>
            {
                getPackageDto(actualStatus: actualStatusKey)
            };

            mockPackageDao.Setup(x => x.GetPackageByTrackingCodesAsync(trackingCodes)).ReturnsAsync(dtos);

            var handler = new PackageQueryHandler(mockPackageDao.Object);

            var result = await handler.GetPackageByTrackingCodesAsync(trackingCodes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(actualStatusValue, result[0].ActualStatus);
        }

        [Theory]
        [InlineData(0, "Passage")]
        [InlineData(1, "Customs")]
        [InlineData(2, "Final Check")]
        [InlineData(3, "")]
        public async Task GetPackageByTrackingCodesAsync_Most_Return_Correct_TypeOfControl(byte typeOfControlKey, string typeOfControlValue)
        {
            var trackingCodes = new string[]
            {
                "trackingCodesTest1",
            };

            var dtos = new List<PackageDto>
            {
                getPackageDto(typeOfControl: typeOfControlKey)
            };

            mockPackageDao.Setup(x => x.GetPackageByTrackingCodesAsync(trackingCodes)).ReturnsAsync(dtos);

            var handler = new PackageQueryHandler(mockPackageDao.Object);

            var result = await handler.GetPackageByTrackingCodesAsync(trackingCodes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(typeOfControlValue, result[0].LastCheckpoint.TypeOfControl);
        }

        [Theory]
        [InlineData(0, "Airport")]
        [InlineData(1, "Port")]
        [InlineData(2, "Station")]
        [InlineData(3, "Customs Facility")]
        [InlineData(4, "External")]
        [InlineData(5, "")]
        public async Task GetPackageByTrackingCodesAsync_Most_Return_Correct_PlaceType(byte placeTypeKey, string placeTypeValue)
        {
            var trackingCodes = new string[]
            {
                "trackingCodesTest1",
            };

            var dtos = new List<PackageDto>
            {
                getPackageDto(placeType: placeTypeKey)
            };

            mockPackageDao.Setup(x => x.GetPackageByTrackingCodesAsync(trackingCodes)).ReturnsAsync(dtos);

            var handler = new PackageQueryHandler(mockPackageDao.Object);

            var result = await handler.GetPackageByTrackingCodesAsync(trackingCodes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(placeTypeValue, result[0].LastCheckpoint.PlaceType);
        }

        [Theory]
        [InlineData(0, "XS")]
        [InlineData(1, "S")]
        [InlineData(2, "M")]
        [InlineData(3, "L")]
        [InlineData(4, "XL")]
        [InlineData(5, "XXL")]
        public async Task GetPackageByTrackingCodesAsync_Most_Return_Correct_Size(byte sizeKey, string sizeValue)
        {
            var trackingCodes = new string[]
            {
                "trackingCodesTest1",
            };

            var dtos = new List<PackageDto>
            {
                getPackageDto(size: sizeKey)
            };

            mockPackageDao.Setup(x => x.GetPackageByTrackingCodesAsync(trackingCodes)).ReturnsAsync(dtos);

            var handler = new PackageQueryHandler(mockPackageDao.Object);

            var result = await handler.GetPackageByTrackingCodesAsync(trackingCodes);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(sizeValue, result[0].Size);
        }

        private PackageDto getPackageDto(byte actualStatus = 1, byte typeOfControl = 2, byte placeType = 1, byte size = 1)
        {
            return new PackageDto
            {
                ActualStatus = actualStatus,
                AreaToDeliver = "AreaToDeliver1",
                HasValueToPay = false,
                Id = 2,
                IsFragile = true,
                IsStoppedInCustoms = false,
                LastCheckpoint = new LastCheckpointDto
                {
                    City = "City1",
                    Country = "Country1",
                    PlaceType = placeType,
                    TypeOfControl = typeOfControl
                },
                PackageWeight = 200,
                Price = 400,
                ReceiveDate = new DateTime(2021, 01, 02),
                Size = size,
                TrackingCode = "TrackingCode1"
            };
        }
    }
}
