using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApp.Models.Response;
using StorageApp.CloudProvider.Config;
using StorageApp.Factory;
using StorageApp.Interfaces;
using StorageApp.Models.ApiResponse;
using System.Runtime.InteropServices;
using StorageApp.CloudProvider.NOSQL;
using StorageApp.Interfaces_Abstract;
using StorageApp.Options.CloudConfig;
using Microsoft.Extensions.Options;
using StorageApp.CloudProvider.RDBMS.Builder;
using StorageApp.Interfaces_Abstract.NoticationService;

namespace StorageApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomExceptionFilter]
    public class NotificationServiceController : ControllerBase
    {
        private readonly NotificationServiceOptions notificationServiceOptions;
        ReturnResponse resp = new ReturnResponse();
        private readonly INotificatoinServiceFactory _notificatoinServiceFactory;
        private readonly INotificationServiceConfiguration noti;
        public NotificationServiceController(IOptionsMonitor<NotificationServiceOptions> options, INotificatoinServiceFactory notificatoinServiceFactory, INotificationServiceConfiguration notification)
        {
            _notificatoinServiceFactory = notificatoinServiceFactory;
            notificationServiceOptions = options.CurrentValue;
            noti = notification;
        }
        [HttpGet("GetNotificationConfig")]
        public async Task<IActionResult> GetNotificationConfig(string cloudname)
        {
            var resp=await noti.GetNotificationServiceConfig(cloudname);
            return Ok(resp);
        }
        [HttpPost("PushMessage")]
        public async Task<IActionResult> PushNotification(string cloudname, string message)
        {
            await _notificatoinServiceFactory.GetNotificatoinProvider(cloudname).SendNotificationAsync(message);
            return Ok("Notification Sent.");
        }
        [HttpPost("PostConfig")]
        public IActionResult PostNotificationServiceConfiguration(NotificationServiceOptions notificatoinconfig)
        {
            if (notificatoinconfig == null)
            {
                return BadRequest("Invalid configuration data.");
            }
            noti.UpdateNotificationService(notificatoinconfig);
            resp.Message = "Updated Successfully";
            return Ok(resp);
        }
    }
}
