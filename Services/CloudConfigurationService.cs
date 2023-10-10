using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StorageApp.CloudProvider.Config;
using StorageApp.Interfaces;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace StorageApp.Services
{
    public class CloudConfigurationService : ICloudConfiguration
    {
        private readonly CloudOptions _cloudoptions;
        string appSettingsFilePath, appSettingsFilePath_BCK;

        public CloudConfigurationService(IOptionsMonitor<CloudOptions> options)
        {
            _cloudoptions = options.CurrentValue;
            appSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            appSettingsFilePath_BCK = Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK", Guid.NewGuid().ToString() + "_appsettings_bck.json");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK"));
            }
        }
        public Task<Object> GetCloudConfig(string cloud)
        {
            PropertyInfo propertyInfo = typeof(CloudOptions).GetProperty(cloud, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
          
            if (propertyInfo != null)
            {
                object cloudOption = propertyInfo.GetValue(_cloudoptions);
                return Task.FromResult(cloudOption);
            }
            object allCloud = _cloudoptions;
            return Task.FromResult(allCloud);
        }

        public void UpdateCloudSettings(CloudOptions cloudOptions)
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

            JObject updatedAzureOptionsObject = JObject.FromObject(cloudOptions.Azure);
            JObject updatedAWSOptionsObject = JObject.FromObject(cloudOptions.AWS);
            jObject["Cloud"]["AWS"] = updatedAWSOptionsObject;
            jObject["Cloud"]["AZURE"] = updatedAzureOptionsObject;
            jObject["Cloud"]["Target"] = cloudOptions.Target;
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }
        public void UpdateCloudSettings(AWSOptions aws)
        {
            
            var json = File.ReadAllText(appSettingsFilePath);
            File.WriteAllText(appSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            JObject updatedAWSOptionsObject = JObject.FromObject(aws);
            jObject["Cloud"]["AWS"] = updatedAWSOptionsObject;            
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }
        public void UpdateCloudSettings(AZUREOptions azure)
        {
            var json = File.ReadAllText(appSettingsFilePath);
            File.WriteAllText(appSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            JObject updatedAWSOptionsObject = JObject.FromObject(azure);
            jObject["Cloud"]["AZURE"] = updatedAWSOptionsObject;
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }

        public void UpdateCloudSettings(string target)
        {
            var json = File.ReadAllText(appSettingsFilePath);
            File.WriteAllText(appSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            jObject["Cloud"]["Target"] = target;
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }
    }
}
