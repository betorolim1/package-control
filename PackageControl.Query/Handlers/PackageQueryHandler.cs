using PackageControl.Query.Handlers.Interface;
using PackageControl.Query.Package.Dao.Dto;
using PackageControl.Query.Package.Dao.Interfaces;
using PackageControl.Query.Package.Result;
using PackageControl.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageControl.Query.Handlers
{
    public class PackageQueryHandler : IPackageQueryHandler
    {
        private IPackageDao _packageDao;

        public PackageQueryHandler(IPackageDao packageDao)
        {
            _packageDao = packageDao;
        }

        public async Task<List<PackageResult>> GetPackageByTrackingCodesAsync(string[] trackingCodes)
        {
            var dtos = await _packageDao.GetPackageByTrackingCodesAsync(trackingCodes);

            var result = new List<PackageResult>();

            foreach (var dto in dtos)
            {
                var lastCheckpointResult = new LastCheckpointResult
                {
                    City = dto.LastCheckpoint.City,
                    Country = dto.LastCheckpoint.Country,
                    PlaceType = getPlaceType(dto.LastCheckpoint.PlaceType),
                    TypeOfControl = getTypeOfControl(dto.LastCheckpoint.TypeOfControl)
                };

                result.Add(new PackageResult
                {
                    ActualStatus = getActualStatus(dto.ActualStatus),
                    HasValueToPay = dto.HasValueToPay,
                    Id = dto.Id,
                    IsFragile = dto.IsFragile,
                    IsStoppedInCustoms = dto.IsStoppedInCustoms,
                    LastCheckpoint = lastCheckpointResult,
                    PackageWeight = dto.PackageWeight,
                    ReceiveDate = dto.ReceiveDate,
                    Size = getSize(dto.Size),
                    TrackingCode = dto.TrackingCode,
                    Price = dto.Price,
                    AreaToDeliver = dto.AreaToDeliver
                });
            }

            return result;
        }

        public async Task<List<string>> GetTrackingCodesByStatusAsync(byte statusNumber)
        {
            var list = await _packageDao.GetTrackingCodesByStatusAsync(statusNumber);

            return list;
        }

        public async Task<List<string>> GetTrackingCodesByPlaceTypeAsync(byte placeType)
        {
            var list = await _packageDao.GetTrackingCodesByPlaceTypeAsync(placeType);

            return list;
        }

        public async Task<decimal> GetStatusAmountMoneyAsync(byte statusNumber)
        {
            var total = await _packageDao.GetStatusAmountMoneyAsync(statusNumber);

            return total;
        }

        private string getActualStatus(byte actualStatus)
        {
            switch (actualStatus)
            {
                case (byte)ActualStatus.Received:
                    return "Received";

                case (byte)ActualStatus.InTransit:
                    return "In Transit";

                case (byte)ActualStatus.StoppedByLegal:
                    return "Stopped By Legal";

                case (byte)ActualStatus.AttempedDelivery:
                    return "Attemped Delivery";

                case (byte)ActualStatus.Returning:
                    return "Returning";

                case (byte)ActualStatus.Delivered:
                    return "Delivered";

                default:
                    return "";
            }
        }

        private string getTypeOfControl(byte typeOfControl)
        {
            switch (typeOfControl)
            {
                case (byte)TypeOfControl.Customs:
                    return "Customs";

                case (byte)TypeOfControl.FinalCheck:
                    return "Final Check";

                case (byte)TypeOfControl.Passage:
                    return "Passage";

                default:
                    return "";
            }
        }

        private string getPlaceType(byte placeType)
        {
            switch (placeType)
            {
                case (byte)PlaceType.Airport:
                    return "Airport";

                case (byte)PlaceType.CustomsFacility:
                    return "Customs Facility";

                case (byte)PlaceType.External:
                    return "External";

                case (byte)PlaceType.Port:
                    return "Port";

                case (byte)PlaceType.Station:
                    return "Station";

                default:
                    return "";
            }
        }

        private string getSize(byte size)
        {
            return ((Size)size).ToString();
        }
    }
}
