using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StorageApp.CloudProvider.NOSQL;
using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract;
using StorageApp.Models.NOSQL;
using StorageApp.Models.Response;
using StorageApp.Services.NOSQL;

namespace StorageApp.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [CustomExceptionFilter]
    public class NOSQLConfigController : Controller
    {
        private readonly NOSQLOptions NOSQLOptions;
        ReturnResponse resp = new ReturnResponse();
        private readonly INOSQLBuilderFactory NOSQLBuilderFactory;
        public NOSQLConfigController(IOptionsMonitor<NOSQLOptions> NOSQLoptions,INOSQLBuilderFactory NOSQLBuilderFactory)
        {
            NOSQLOptions = NOSQLoptions.CurrentValue;
            this.NOSQLBuilderFactory = NOSQLBuilderFactory;
        }
        [HttpPost]
        public async Task<IActionResult> PostDB(NOSQLInfo NOSQL,string cloudName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            director.Construct(NOSQL);
            return Ok("DB Posted");
        }
        [HttpPost]
        public async Task<IActionResult> PostConnection(Connections connections,string cloudName,string dbName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            director.ConstructConnection(connections, dbName);
            return Ok("Connection Added");
        }
        [HttpGet]
        public async Task<IActionResult> GetDB(string cloudName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            var DBS=director.GetDB();
            return Ok(DBS);
        }
        [HttpGet]
        public async Task<IActionResult> GetConnections(string cloudName,string dbName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            var connections = director.GetConnections(dbName);
            return Ok(connections);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNOSQL()
        {
            return Ok(NOSQLOptions);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDB(string cloudName, string dbName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            director.DeleteDB( dbName);
            return Ok("DB Deleted");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConnection(string cloudName,string dbName, string connectionname)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            director.DeleteConnection(dbName,connectionname);
            return Ok("Connection Deleted");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateConnection(Connections connections, string cloudName, string dbName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            director.UpdateConnection(connections, dbName);
            return Ok("Connection Updated");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDB(NOSQLInfo NOSQL, string cloudName)
        {
            var nosqlbuilder = NOSQLBuilderFactory.GetNOSQLBuilder(cloudName);
            var director = new NOSQLDirector(nosqlbuilder);
            director.UpdateDB(NOSQL);
            return Ok("DB Updated");
        }
    }
}
