using Microsoft.AspNetCore.Mvc;
using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;
using StorageApp.Interfaces;
using StorageApp.Models.Response;

namespace StorageApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    public class RDBMSConfigController : Controller
    {
        private readonly IRDBMSConfiguration rdbmsConfiguration;
        ReturnResponse resp = new ReturnResponse();
        public RDBMSConfigController(IRDBMSConfiguration rdbmsConfiguration)
        {
            this.rdbmsConfiguration = rdbmsConfiguration;
        }

        [HttpGet]
        public async Task<IActionResult> GetRDBMSonfiguration(string cloudname)
        {
            if (string.IsNullOrEmpty(cloudname))
            {
                return BadRequest("Cloud name is missing.");
            }
            var config = await rdbmsConfiguration.GetRDBMSConfig(cloudname);
            if (config == null)
            {
                return NotFound("RDBMS configuration not found.");
            }
            return Ok(config);
        }
        [HttpPost]
        public IActionResult PostRDBMSConfiguration(RDBMSOptions rdbms)
        {
            if (rdbms == null)
            {
                return BadRequest("Invalid RDBMS configuration data.");
            }
            rdbmsConfiguration.UpdateRDBMSSettings(rdbms);
            resp.Message = "Updated Successfully";
            return Ok(resp);
        }
        [HttpPost("target")]
        public IActionResult PostTarget(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return BadRequest("Invalid target.");
            }
            rdbmsConfiguration.UpdateRDBMSSettings(target);
            resp.Message = "Target Updated Successfully";
            return Ok(resp);
        }
    }
}
