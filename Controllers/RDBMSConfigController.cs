using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;
using StorageApp.CloudProvider.RDBMS.Builder;
using StorageApp.Interfaces;
using StorageApp.Models.RDBMS;
using StorageApp.Models.Response;
using StorageApp.Services;
using StorageApp.Services.RDBMS;

namespace StorageApp.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [CustomExceptionFilter]
    public class RDBMSConfigController : Controller
    {
        private readonly IRDBMSConfiguration rdbmsConfiguration;
        private readonly IRDBMSBuilder rdbmsbuilder;
        private readonly RDBMSOptions rDBMSOptions;
        ReturnResponse resp = new ReturnResponse();
        public RDBMSConfigController(IRDBMSBuilder rdbmsConfiguration,IOptionsMonitor<RDBMSOptions> rdbmsoptions)
        {
            rDBMSOptions = rdbmsoptions.CurrentValue;
        }
        [HttpPost]
        public async Task<IActionResult> PostDB(RDBMSInfo rdbms)
        {
            var azureBuilder = new AZUREBuilder();
            var director = new RDBMSDirector(azureBuilder);
            director.Construct(rdbms);
            var dbList = azureBuilder.GetListDataBase();
            return Ok(dbList);
        }
        [HttpPost]
        public async Task<IActionResult> PostConnection(Connections connections,string dbname)
        {
            var azureBuilder = new AZUREBuilder();
            var director = new RDBMSDirector(azureBuilder);
            director.ConstructConnection(connections, dbname);
            var dbList = azureBuilder.GetListDataBase();
            return Ok(dbList);
        }
        //[HttpPost]
        //public IActionResult PostRDBMSConfiguration(RDBMSOptions rdbms)
        //{
        //    if (rdbms == null)
        //    {
        //        return BadRequest("Invalid RDBMS configuration data.");
        //    }
        //    rdbmsConfiguration.UpdateRDBMSSettings(rdbms);
        //    resp.Message = "Updated Successfully";
        //    return Ok(resp);
        //}
        //[ApiExplorerSettings(IgnoreApi = true)]
        //[HttpPost("target")]
        //public IActionResult PostTarget(string target)
        //{
        //    if (string.IsNullOrEmpty(target))
        //    {
        //        return BadRequest("Invalid target.");
        //    }
        //    rdbmsConfiguration.UpdateRDBMSSettings(target);
        //    resp.Message = "Target Updated Successfully";
        //    return Ok(resp);
        //}
    }
}
