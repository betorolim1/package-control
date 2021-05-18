using Dapper;
using PackageControl.Query.Helper;
using PackageControl.Query.Package.Dao.Dto;
using PackageControl.Query.Package.Dao.Interfaces;
using PackageControl.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageControl.Data.Dao
{
    public class PackageDao : IPackageDao
    {
        private DbSession _session;

        public PackageDao(DbSession session)
        {
            _session = session;
        }

        public async Task<List<PackageDto>> GetPackageByTrackingCodesAsync(string[] trackingCodes)
        {
            var list = new List<PackageDto>();

            await _session.OpenAsync();

            try
            {
                using (var reader = await _session.Connection.ExecuteReaderAsync(
                    sql: $@"
                            SELECT [PackageId]
                                  ,[ReceiveDate]
                                  ,[IsStoppedInCustoms]
                                  ,[HasValueToPay]
                                  ,[TrackingCode]
                                  ,[PackageWeight]
                                  ,[Size]
                                  ,[IsFragile]
                                  ,[ActualStatus]
								  ,[Country]
								  ,[City]
								  ,[TypeOfControl]
								  ,[PlaceType]
                                  ,[Price]
                                  ,[AreaToDeliver]
                            FROM [Package] pc
                            INNER JOIN LastCheckpoint lc ON lc.LastCheckpointId = pc.LastCheckpointId
                            WHERE TrackingCode in @TrackingCodes
                        ",
                    param: new
                    {
                        TrackingCodes = trackingCodes
                    }
                    ))
                {
                    while (await reader.ReadAsync())
                    {
                        var lastCheckpoint = new LastCheckpointDto
                        {
                            City = await reader.GetValueOrDefaultAsync<string>("City"),
                            Country = await reader.GetValueOrDefaultAsync<string>("Country"),
                            PlaceType = await reader.GetValueOrDefaultAsync<byte>("PlaceType"),
                            TypeOfControl = await reader.GetValueOrDefaultAsync<byte>("TypeOfControl")
                        };

                        list.Add(new PackageDto
                        {
                            ActualStatus = await reader.GetValueOrDefaultAsync<byte>("ActualStatus"),
                            HasValueToPay = await reader.GetValueOrDefaultAsync<bool>("HasValueToPay"),
                            Id = await reader.GetValueOrDefaultAsync<long>("PackageId"),
                            IsFragile = await reader.GetValueOrDefaultAsync<bool>("IsFragile"),
                            IsStoppedInCustoms = await reader.GetValueOrDefaultAsync<bool>("IsStoppedInCustoms"),
                            ReceiveDate = await reader.GetValueOrDefaultAsync<DateTime>("ReceiveDate"),
                            PackageWeight = await reader.GetValueOrDefaultAsync<int>("PackageWeight"),
                            Size = await reader.GetValueOrDefaultAsync<byte>("Size"),
                            TrackingCode = await reader.GetValueOrDefaultAsync<string>("TrackingCode"),
                            Price = await reader.GetValueOrDefaultAsync<decimal>("Price"),
                            AreaToDeliver = await reader.GetValueOrDefaultAsync<string>("AreaToDeliver"),
                            LastCheckpoint = lastCheckpoint
                        });
                    }
                }

                return list;
            }
            finally
            {
                await _session.CloseAsync();
            }
        }

        public async Task<List<string>> GetTrackingCodesByStatusAsync(byte statusNumber)
        {
            var list = new List<string>();

            await _session.OpenAsync();

            try
            {
                using (var reader = await _session.Connection.ExecuteReaderAsync(
                    sql: $@"
                             SELECT [TrackingCode]
                             FROM [Package]
                             WHERE ActualStatus = @StatusNumber
                        ",
                    param: new
                    {
                        StatusNumber = statusNumber
                    }
                    ))
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(await reader.GetValueOrDefaultAsync<string>("TrackingCode"));
                    }
                }

                return list;
            }
            finally
            {
                await _session.CloseAsync();
            }
        }

        public async Task<List<string>> GetTrackingCodesByPlaceTypeAsync(byte placeType)
        {
            var list = new List<string>();

            await _session.OpenAsync();

            try
            {
                using (var reader = await _session.Connection.ExecuteReaderAsync(
                    sql: $@"
                              SELECT [TrackingCode]
                              FROM [LastCheckpoint] lc
                              INNER JOIN Package pc ON pc.LastCheckpointId = lc.LastCheckpointId
                              WHERE PlaceType = @PlaceType
                        ",
                    param: new
                    {
                        PlaceType = placeType
                    }
                    ))
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(await reader.GetValueOrDefaultAsync<string>("TrackingCode"));
                    }
                }

                return list;
            }
            finally
            {
                await _session.CloseAsync();
            }
        }

        public async Task<decimal> GetStatusAmountMoneyAsync(byte statusNumber)
        {
            decimal total = 0;

            await _session.OpenAsync();

            try
            {
                using (var reader = await _session.Connection.ExecuteReaderAsync(
                    sql: $@"
                              SELECT Price
                              FROM [Package]
                              WHERE ActualStatus = @StatusNumber
                        ",
                    param: new
                    {
                        StatusNumber = statusNumber
                    }
                    ))
                {
                    while (await reader.ReadAsync())
                    {
                        total += await reader.GetValueOrDefaultAsync<decimal>("Price");
                    }
                }

                return total;
            }
            finally
            {
                await _session.CloseAsync();
            }
        }
    }
}
