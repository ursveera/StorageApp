using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApp.Factory;
using StorageApp.Models.NOSQL;
using StorageApp.Services.NOSQL;

namespace StorageApp.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> asdfasdfDe1111()
        {
            return Ok("DB Posted");
        }
    }
}
