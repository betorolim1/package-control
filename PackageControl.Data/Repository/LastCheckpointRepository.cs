using Dapper;
using PackageControl.Core.LastCheckpoint;
using PackageControl.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace PackageControl.Data.Repository
{
    public class LastCheckPointRepository : ILastCheckpointRepository
    {
        private DbSession _session;

        public LastCheckPointRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<long> InsertLastCheckpoint(string country, string city, byte typeOfControl, byte placeType)
        {
            await _session.OpenAsync();

            var query = await _session.Connection.QueryAsync<long>(
                     sql: $@"
                           INSERT INTO LastCheckpoint(
                                Country,
                                City,
                                TypeOfControl,
                                PlaceType
                           ) VALUES (
                               @Country,
                               @City,
                               @TypeOfControl,
                               @PlaceType )

                            SELECT CAST(SCOPE_IDENTITY() as BIGINT);
                        ",
                     param: new
                     {
                         Country = country,
                         City = city,
                         TypeOfControl = typeOfControl,
                         PlaceType = placeType
                     },
                     transaction: _session.Transaction
                     );

            return query.Single();
        }
    }
}
