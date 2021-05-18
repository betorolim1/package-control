using Dapper;
using PackageControl.Core.Package.Commands;
using PackageControl.Core.Package.Repository;
using PackageControl.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PackageControl.Data.Repository
{
    public class PackageRepository : IPackagesRepository
    {
        private DbSession _session;

        public PackageRepository(DbSession session)
        {
            _session = session;
        }

        public async Task UpdatePackagesAsync(UpdatePackageCommand package, long lastCheckpointId)
        {
            await _session.OpenAsync();

            await _session.Connection.ExecuteAsync(
                    sql: $@"
                            UPDATE Package
                            SET 
                                IsStoppedInCustoms = @IsStoppedInCustoms,
                                HasValueToPay = @HasValueToPay,
                                PackageWeight = @PackageWeight,
                                Size = @Size,
                                IsFragile = @IsFragile,
                                ActualStatus = @ActualStatus,
                                Price = @Price,
                                AreaToDeliver = @AreaToDeliver,
                                LastCheckpointId = @LastCheckpointId
                            WHERE 
                                TrackingCode = @TrackingCode
                        ",
                    param: new
                    {
                        package.ActualStatus,
                        package.HasValueToPay,
                        package.IsFragile,
                        package.IsStoppedInCustoms,
                        package.PackageWeight,
                        package.Price,
                        package.Size,
                        package.TrackingCode,
                        package.AreaToDeliver,
                        LastCheckpointId = lastCheckpointId
                    },
                    transaction: _session.Transaction
                    );
        }

        public async Task<bool> HasPackageByTrackingCodeAsync(string trackingCode)
        {
            await _session.OpenAsync();

            using (var reader = await _session.Connection.ExecuteReaderAsync(
            sql: $@"
                             SELECT TOP (1) [TrackingCode]
                             FROM [Package]
                             WHERE TrackingCode = @TrackingCode
                        ",
            param: new
            {
                TrackingCode = trackingCode
            },
            transaction: _session.Transaction
            ))
            {
                while (reader.Read())
                {
                    return true;
                }
            }

            return false;
        }

        public async Task InsertPackagesAsync(string trackingCode, InsertPackageCommand package, long lastCheckpointId)
        {
            await _session.OpenAsync();

            await _session.Connection.QueryAsync<long>(
                     sql: $@"
                           INSERT INTO Package(
                              [LastCheckpointId]
                              ,[ReceiveDate]
                              ,[IsStoppedInCustoms]
                              ,[HasValueToPay]
                              ,[TrackingCode]
                              ,[PackageWeight]
                              ,[Size]
                              ,[IsFragile]
                              ,[ActualStatus]
                              ,[Price]
                              ,[AreaToDeliver])
	                       VALUES(
                                @LastCheckpointId,
                                @ReceivedDate,
                                @IsStoppedInCustoms,
                                @HasValueToPay,
                                @TrackingCode,
                                @PackageWeight,
                                @Size,
                                @IsFragile,
                                @ActualStatus,
                                @Price,
                                @AreaToDeliver
	                          )
                        ",
                     param: new
                     {
                         package.ActualStatus,
                         package.HasValueToPay,
                         package.IsFragile,
                         package.IsStoppedInCustoms,
                         package.PackageWeight,
                         package.Price,
                         package.Size,
                         package.AreaToDeliver,
                         package.ReceivedDate,
                         TrackingCode = trackingCode,
                         LastCheckpointId = lastCheckpointId
                     },
                     transaction: _session.Transaction
                     );
        }
    }
}
