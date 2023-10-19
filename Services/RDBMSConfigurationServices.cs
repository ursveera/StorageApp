using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS;
using StorageApp.Interfaces;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace StorageApp.Services
{
    public class RDBMSConfigurationServices:IRDBMSConfiguration
    {
        private readonly RDBMSOptions _rdbmsoptions;
        string appSettingsFilePath, appSettingsFilePath_BCK;

        public RDBMSConfigurationServices(IOptionsMonitor<RDBMSOptions> options)
        {
            _rdbmsoptions = options.CurrentValue;
            appSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            appSettingsFilePath_BCK = Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK", Guid.NewGuid().ToString() + "_appsettings_bck.json");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK"));
            }
        }

        public Task<object> GetRDBMSConfig(string rdbms)
        {
            var propertyInfo = typeof(RDBMSOptions).GetProperty(rdbms, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            object? rdbmsOption = propertyInfo != null ? propertyInfo.GetValue(_rdbmsoptions) : _rdbmsoptions;
            return Task.FromResult<object>(rdbmsOption);
        }

        public void UpdateRDBMSSettings(RDBMSOptions rDBMSOptions)
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
            JObject updatedmssqlOptionsObject = JObject.FromObject(rDBMSOptions.MicrosoftSQLServer);
            JObject updatedmysqlOptionsObject = JObject.FromObject(rDBMSOptions.MySQL);
            jObject["RDBMS"]["MicrosoftSQLServer"] = updatedmssqlOptionsObject;
            jObject["RDBMS"]["MySQL"] = updatedmysqlOptionsObject;
            jObject["RDBMS"]["Target"] = rDBMSOptions.Target;
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }

        public void UpdateRDBMSSettings(string target)
        {

            var json = File.ReadAllText(appSettingsFilePath);
            File.WriteAllText(appSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            jObject["RDBMS"]["Target"] = target;
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePath, updatedJson);
        }
    }
}
