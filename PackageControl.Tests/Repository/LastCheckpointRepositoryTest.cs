using Microsoft.Extensions.Configuration;
using Moq;
using PackageControl.Data.Repository;
using PackageControl.Shared;
using System.Threading.Tasks;
using Xunit;

namespace PackageControl.Tests.Repository
{
    public class LastCheckpointRepositoryTest
    {
        private Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();

        private DbSession session;

        public LastCheckpointRepositoryTest()
        {
            mockConfig.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value).Returns("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=PackageControl;Data Source=DESKTOP-123456");

            session = new DbSession(mockConfig.Object);
        }

        [Fact(Skip = "DataBase test")]
        public async Task InsertLastCheckpoint_Must_Insert_LastCheckpoint()
        {
            var dao = new LastCheckPointRepository(session);

            var result = await dao.InsertLastCheckpoint("CountryLastCheckpointRepositoryTest", "CityLastCheckpointRepositoryTest", 1, 2);

            Assert.True(result > 0);
        }
    }
}
