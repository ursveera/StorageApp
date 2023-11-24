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
        private readonly RDBMSOptions rDBMSOptions;
        ReturnResponse resp = new ReturnResponse();
        private readonly IRDBMSBuilderFactory rDBMSBuilderFactory;
        public RDBMSConfigController(IOptionsMonitor<RDBMSOptions> rdbmsoptions,IRDBMSBuilderFactory rDBMSBuilderFactory)
        {
            rDBMSOptions = rdbmsoptions.CurrentValue;
            this.rDBMSBuilderFactory = rDBMSBuilderFactory;
        }
        [HttpPost]
        public async Task<IActionResult> PostDB(RDBMSInfo rdbms,string cloudName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.Construct(rdbms);
            return Ok("DB Posted");
        }
        [HttpPost]
        public async Task<IActionResult> PostConnection(Connections connections,string cloudName,string dbName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.ConstructConnection(connections, dbName);
            return Ok("Connection Added");
        }
        [HttpGet]
        public async Task<IActionResult> GetDB(string cloudName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            var DBS=director.GetDB();
            return Ok(DBS);
        }
        [HttpGet]
        public async Task<IActionResult> GetConnections(string cloudName,string dbName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            var connections = director.GetConnections(dbName);
            return Ok(connections);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRDBMS()
        {
            return Ok(rDBMSOptions);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDB(string cloudName, string dbName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.DeleteDB( dbName);
            return Ok("DB Deleted");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConnection(string cloudName,string dbName, string connectionname)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.DeleteConnection(dbName,connectionname);
            return Ok("Connection Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateConnection(Connections connections, string cloudName, string dbName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.UpdateConnection(connections, dbName);
            return Ok("Connection Updated");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDB(RDBMSInfo rdbms, string cloudName)
        {
            var rdmbsbuilder = rDBMSBuilderFactory.GetRDGMSBuiler(cloudName);
            var director = new RDBMSDirector(rdmbsbuilder);
            director.UpdateDB(rdbms);
            return Ok("DB Updated");
        }
    }
}
