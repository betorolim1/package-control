using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace package_control.Controllers
{
    [Route("packages")]
    public class PackagesController : Controller
    {
        public PackagesController()
        {

        }

        [HttpGet]
        public async Task<ActionResult> GetPackage()
        {
            return Ok();
        }
    }
}
