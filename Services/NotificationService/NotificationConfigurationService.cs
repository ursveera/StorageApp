using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StorageApp.CloudProvider.Config;
using StorageApp.Interfaces;
using StorageApp.Interfaces_Abstract.NoticationService;
using StorageApp.Options.CloudConfig;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace StorageApp.Services
{
    public class NotificationConfigurationService : INotificationServiceConfiguration
    {
        private readonly NotificationServiceOptions _cloudoptions;
        string appSettingsFilePath, appSettingsFilePath_BCK;
        public NotificationConfigurationService(IOptionsMonitor<NotificationServiceOptions> options)
        {
            _cloudoptions = options.CurrentValue;
            appSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            appSettingsFilePath_BCK = Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK", Guid.NewGuid().ToString() + "_appsettings_bck.json");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK"));
            }
        }
        public Task<object> GetNotificationServiceConfig(string cloud)
        {
            var propertyInfo = typeof(NotificationServiceOptions).GetProperty(cloud, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            object? cloudOption = propertyInfo != null? propertyInfo.GetValue(_cloudoptions): _cloudoptions;
            return Task.FromResult<object>(cloudOption);
        }

        public void UpdateNotificationService(NotificationServiceOptions cloudOptions)
        {
            var appSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var appSettingsFilePath_BCK = Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK", Guid.NewGuid().ToString() + "_appsettings_bck.json");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK"));
            }
            var json = File.ReadAllText(appSettingsFilePath);
            File.WriteAllText(appSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            JObject updatedAzureOptionsObject = JObject.FromObject(cloudOptions.azure);
            JObject updatedAWSOptionsObject = JObject.FromObject(cloudOptions.aws);
            JObject updatedGcpOptionsObject = JObject.FromObject(cloudOptions.gcp);
            jObject["NotificationService"]["AWS"] = updatedAWSOptionsObject;
            jObject["NotificationService"]["AZURE"] = updatedAzureOptionsObject;
            jObject["NotificationService"]["GCP"] = updatedGcpOptionsObject;
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }
       
    }
}
