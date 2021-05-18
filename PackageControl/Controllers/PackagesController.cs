using Microsoft.AspNetCore.Mvc;
using PackageControl.Core.Handlers.Interfaces;
using PackageControl.Core.Package.Commands;
using PackageControl.Query.Handlers.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace package_control.Controllers
{
    [Route("packages")]
    public class PackagesController : Controller
    {
        private IPackageQueryHandler _packageQueryHandler;
        private IPackageHandler _packageHandler;

        public PackagesController(IPackageQueryHandler packageQueryHandler, IPackageHandler packageHandler)
        {
            _packageQueryHandler = packageQueryHandler;
            _packageHandler = packageHandler;
        }

        [HttpGet("trackingcodes")]
        public async Task<ActionResult> GetPackageByTrankingCodeListAsync([FromQuery] string[] trackingCodes)
        {
            var result = await _packageQueryHandler.GetPackageByTrackingCodesAsync(trackingCodes);

            return Ok(result);
        }

        [HttpGet("trackingcode/{trackingCode}")]
        public async Task<ActionResult> GetPackageByTrankingCodeAsync(string trackingCode)
        {
            var result = await _packageQueryHandler.GetPackageByTrackingCodesAsync(new string[] { trackingCode });

            return Ok(result);
        }

        [HttpGet("status/{statusNumber}")]
        public async Task<ActionResult> GetTrankingCodesByStatusAsync(byte statusNumber)
        {
            var result = await _packageQueryHandler.GetTrackingCodesByStatusAsync(statusNumber);

            return Ok(result);
        }

        [HttpGet("placetype/{placeType}")]
        public async Task<ActionResult> GetTrackingCodesByPlaceTypeAsync(byte placeType)
        {
            var result = await _packageQueryHandler.GetTrackingCodesByPlaceTypeAsync(placeType);

            return Ok(result);
        }

        [HttpGet("statusamountmoney/{statusNumber}")]
        public async Task<ActionResult> GetStatusAmountMoneyAsync(byte statusNumber)
        {
            var result = await _packageQueryHandler.GetStatusAmountMoneyAsync(statusNumber);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePackagesAsync([FromBody] List<UpdatePackageCommand> list)
        {
            if (list is null)
                return BadRequest();

           await _packageHandler.UpdatePackagesAsync(list);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> InsertPackagesAsync([FromBody] List<InsertPackageCommand> list)
        {
            if (list is null)
                return BadRequest();

           await _packageHandler.InsertPackagesAsync(list);

            return Ok();
        }
    }
}
