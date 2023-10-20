using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApp.Models.Response;
using StorageApp.CloudProvider.Config;
using StorageApp.Factory;
using StorageApp.Interfaces;
using StorageApp.Models.ApiResponse;
using System.Runtime.InteropServices;

namespace StorageApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    public class CloudConfigController : ControllerBase
    {
        private readonly ICloudConfiguration cloudConfiguration;
        ReturnResponse resp = new ReturnResponse();

        public CloudConfigController(ICloudConfiguration cloudConfiguration)
        {
            this.cloudConfiguration = cloudConfiguration;
        }

        /// <summary>
        /// This is a GetCloudConfiguration API method that does something
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/CloudConfig
        ///
        /// </remarks>
        /// <returns>Some sample data</returns>
        [HttpGet]
        public async Task<IActionResult> GetCloudConfiguration(string cloudname)
        {
            if (string.IsNullOrEmpty(cloudname))
            {
                return BadRequest("Cloud name is missing.");
            }
            var config = await cloudConfiguration.GetCloudConfig(cloudname);
            if (config == null)
            {
                return NotFound("Cloud configuration not found.");
            }
            return Ok(config);
        }
        [HttpPost]
        public IActionResult PostCloudConfiguration(CloudOptions cloud)
        {
            if (cloud == null)
            {
                return BadRequest("Invalid cloud configuration data.");
            }
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "Updated Successfully";
            return Ok(resp);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("aws")]
        public IActionResult PostAWS(AWSOptions cloud)
        {
            if (cloud == null)
            {
                return BadRequest("Invalid cloud configuration data.");
            }
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "AWS Updated Successfully";
            return Ok(resp);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("azure")]
        public IActionResult PostAZURE(AZUREOptions cloud)
        {
            if (cloud == null)
            {
                return BadRequest("Invalid cloud configuration data.");
            }
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "Azure Updated Successfully";
            return Ok(resp);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("gcp")]
        public IActionResult PostGCP(GCPOptions cloud)
        {
            if (cloud == null)
            {
                return BadRequest("Invalid cloud configuration data.");
            }
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "Gcp Updated Successfully";
            return Ok(resp);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("target")]
        public IActionResult PostTarget(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return BadRequest("Invalid target.");
            }
            cloudConfiguration.UpdateCloudSettings(target);
            resp.Message = "Target Updated Successfully";
            return Ok(resp);
        }
    }
}
