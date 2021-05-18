using Microsoft.Extensions.Configuration;
using PackageControl.Core.Country;
using PackageControl.Core.Country.Service;
using PackageControl.Core.Handlers.Interfaces;
using PackageControl.Core.LastCheckpoint;
using PackageControl.Core.Package.Commands;
using PackageControl.Core.Package.Repository;
using PackageControl.Shared;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace PackageControl.Core.Handlers
{
    public class PackageHandler : IPackageHandler
    {
        private const int AREA_TO_DELIVER_TRACKING_CODE_LENGTH = 7;
        private const int GUID_TRACKING_CODE_LENGTH = 6;

        private IPackagesRepository _packageRepository;
        private ILastCheckpointRepository _lastCheckpointRepository;
        private IUnitOfWork _unitOfWork;
        private ICountryService _countryService;

        public PackageHandler(IPackagesRepository packageRepository, ILastCheckpointRepository lastCheckpointRepository, IUnitOfWork unitOfWork, ICountryService countryService)
        {
            _packageRepository = packageRepository;
            _lastCheckpointRepository = lastCheckpointRepository;
            _unitOfWork = unitOfWork;
            _countryService = countryService;
        }

        public async Task<List<string>> InsertPackagesAsync(List<InsertPackageCommand> insertPackages)
        {
            var trackingCodes = new List<string>();
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var package in insertPackages)
                {
                    var countryData = await _countryService.GetCountryData(package.Country);

                    if (countryData is null || countryData.NumericCode is null || countryData.Alpha2Code is null)
                        throw new Exception($"Country {package.Country} not found");

                    var trackingCode = getTrackingCode(countryData, package.AreaToDeliver, package.ReceivedDate);

                    var lastCheckpointId = await _lastCheckpointRepository.InsertLastCheckpoint(package.Country, package.City, package.TypeOfControl, package.PlaceType);

                    await _packageRepository.InsertPackagesAsync(trackingCode, package, lastCheckpointId);

                    trackingCodes.Add(trackingCode);
                }

                _unitOfWork.Commit();

                return trackingCodes;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public async Task UpdatePackagesAsync(List<UpdatePackageCommand> updatePackages)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                foreach (var package in updatePackages)
                {
                    var hasPackage = await _packageRepository.HasPackageByTrackingCodeAsync(package.TrackingCode);

                    if (!hasPackage)
                    {
                        throw new Exception(package.TrackingCode + " does not exists");
                    }

                    var lastCheckpointId = await _lastCheckpointRepository.InsertLastCheckpoint(package.Country, package.City, package.TypeOfControl, package.PlaceType);

                    await _packageRepository.UpdatePackagesAsync(package, lastCheckpointId);
                }

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        private string getTrackingCode(CountryDto countryData, string areaToDeliver, DateTime receivedDate)
        {
            var isoCountryCode = countryData.Alpha2Code;

            var formateAreaToDeliverd = getAreaToDeliverTrackingCode(areaToDeliver);

            var guid = getGuid();

            var formatedReceivedDate = receivedDate.ToString("ddMMyy");

            var t0 = getT0(formatedReceivedDate);

            var t1 = getT1(receivedDate, countryData.NumericCode);

            return $"{isoCountryCode}-{formateAreaToDeliverd}-{guid}-{formatedReceivedDate}-{t0}{t1}";
        }

        private int getT1(DateTime receivedDate, string numericCode)
        {
            var y1 = int.Parse(receivedDate.ToString("yy").Substring(1, 1));

            var numericCodePlusY1 = int.Parse(numericCode) + y1;

            return sumDigit(numericCodePlusY1);
        }

        private int getT0(string formatedReceivedDate)
        {
            var total = (int.Parse(formatedReceivedDate.Substring(0, 1)) + int.Parse(formatedReceivedDate.Substring(3, 1)) + 20) * 2;

            return sumDigit(total);
        }

        private string getGuid()
        {
            var guidString = Guid.NewGuid().ToString();
            return guidString.Substring(guidString.Length - GUID_TRACKING_CODE_LENGTH, GUID_TRACKING_CODE_LENGTH);
        }

        private string getAreaToDeliverTrackingCode(string areaToDeliver)
        {
            var reverseAreaToDeliver = reverse(areaToDeliver);

            string formatedAreaToDeliver;

            if (reverseAreaToDeliver.Length < AREA_TO_DELIVER_TRACKING_CODE_LENGTH)
            {
                formatedAreaToDeliver = reverseAreaToDeliver.Substring(0, reverseAreaToDeliver.Length).PadLeft(AREA_TO_DELIVER_TRACKING_CODE_LENGTH, '0');
            }
            else
            {
                formatedAreaToDeliver = reverseAreaToDeliver.Substring(0, AREA_TO_DELIVER_TRACKING_CODE_LENGTH);
            }

            return formatedAreaToDeliver;
        }

        private int sumDigit(int total)
        {
            var input = total;
            int result = input % 9;
            return (result == 0 && input > 0) ? 9 : result;
        }

        private string reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
