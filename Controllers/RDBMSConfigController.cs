using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;
using StorageApp.CloudProvider.RDBMS.Builder;
using StorageApp.Factory;
using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract;
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
        private readonly IRDBMSBuilderFactory rDBMSBuilderFactory;
        public RDBMSConfigController(IRDBMSBuilder rdbmsConfiguration,IOptionsMonitor<RDBMSOptions> rdbmsoptions,IRDBMSBuilderFactory rDBMSBuilderFactory)
        {
            rDBMSOptions = rdbmsoptions.CurrentValue;
            this.rDBMSBuilderFactory = rDBMSBuilderFactory;
        }
        [HttpPost]
        public async Task<IActionResult> PostDB(RDBMSInfo rdbms,string cloudname)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudname);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.Construct(rdbms);
            return Ok("DB Posted");
        }
        [HttpPost]
        public async Task<IActionResult> PostConnection(Connections connections,string cloudname,string dbname)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudname);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.ConstructConnection(connections, dbname);
            return Ok("Connection Added");
        }
        [HttpGet]
        public async Task<IActionResult> GetDB(string cloudname)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudname);
            var director = new RDBMSDirector(rdmbsbuilder);
            var DBS=director.GetDB();
            return Ok(DBS);
        }
        [HttpGet]
        public async Task<IActionResult> GetConnections(string cloudname,string dbname)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudname);
            var director = new RDBMSDirector(rdmbsbuilder);
            var connections = director.GetConnections(dbname);
            return Ok(connections);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRDBMS()
        {
            return Ok(rDBMSOptions);
        }
    }
}
