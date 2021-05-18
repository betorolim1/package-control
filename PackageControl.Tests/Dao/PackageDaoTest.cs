using Microsoft.Extensions.Configuration;
using Moq;
using PackageControl.Data.Dao;
using PackageControl.Shared;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests.Dao
{
    public class PackageDaoTest
    {
        private Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();

        private DbSession session;

        public PackageDaoTest()
        {
            mockConfig.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value).Returns("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PackageControl;Data Source=DESKTOP-123456");

            session = new DbSession(mockConfig.Object);
        }

        [Fact(Skip = "DataBase test")]
        public async Task GetPackageByTrackingCodesAsync_Must_Return_Package()
        {
            var dao = new PackageDao(session);

            var result = await dao.GetPackageByTrackingCodesAsync(new string[] { "teste-teste" });

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact(Skip = "DataBase test")]
        public async Task GetTrackingCodesByStatusAsync_Must_Return_TrackingCodes()
        {
            var dao = new PackageDao(session);

            var result = await dao.GetTrackingCodesByStatusAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact(Skip = "DataBase test")]
        public async Task GetTrackingCodesByPlaceTypeAsync_Must_Return_TrackingCodes()
        {
            var dao = new PackageDao(session);

            var result = await dao.GetTrackingCodesByPlaceTypeAsync(1);

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        [Fact(Skip = "DataBase test")]
        public async Task GetTrackingCodesByPlaceTypeAsync_Must_Return_Money()
        {
            var dao = new PackageDao(session);

            var result = await dao.GetStatusAmountMoneyAsync(1);

            Assert.True(result > 0);
        }
    }
}
