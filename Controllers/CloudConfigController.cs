using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OKTA.Models.Response;
using StorageApp.CloudProvider.Config;
using StorageApp.Factory;
using StorageApp.Interfaces;
using StorageApp.Models.ApiResponse;

namespace StorageApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudConfigController : ControllerBase
    {
        private readonly ICloudConfiguration cloudConfiguration;
        ReturnResponse resp = new ReturnResponse();

        public CloudConfigController(ICloudConfiguration cloudConfiguration)
        {
            this.cloudConfiguration = cloudConfiguration;
        }

        [HttpGet]
        public IActionResult GetCloudConfiguration(string cloudname)
        {
            var config=cloudConfiguration.GetCloudConfig(cloudname);
            return Ok(config.Result);
        }
        [HttpPost]
        public IActionResult PostCloudConfiguration(CloudOptions cloud)
        {
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "Updated Successfully";
            return Ok(resp);
        }
        [HttpPost("aws")]
        public IActionResult PostAWS(AWSOptions cloud)
        {
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "AWS Updated Successfully";
            return Ok(resp);
        }
        [HttpPost("azure")]
        public IActionResult PostAZURE(AZUREOptions cloud)
        {
            cloudConfiguration.UpdateCloudSettings(cloud);
            resp.Message = "Azure Updated Successfully";
            return Ok(resp);
        }
        [HttpPost("target")]
        public IActionResult PostTarget(string target)
        {
            cloudConfiguration.UpdateCloudSettings(target);
            resp.Message = "Target Updated Successfully";
            return Ok(resp);
        }
    }
}
