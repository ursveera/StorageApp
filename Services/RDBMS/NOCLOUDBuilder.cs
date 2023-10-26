using StorageApp.Models.RDBMS;
using StorageApp.Interfaces;
using StorageApp.CloudProvider.Config;
using StorageApp.CloudProvider.RDBMS.Builder;
using Microsoft.Extensions.Options;
using static Google.Cloud.Storage.V1.UrlSigner;
using StorageApp.FileManager;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace StorageApp.Services
{
    public class NOCLOUDBuilder : IRDBMSBuilder
    {

        private readonly RDBMSOptions _rdbmsoptions;
        private readonly AppSettingsFilePathManager appSettingsFilePathManager;

        public NOCLOUDBuilder()
        {
            appSettingsFilePathManager = AppSettingsFilePathManager.Instance;
        }

        public void AddDataBase(RDBMSInfo info)
        {
            var json = File.ReadAllText(appSettingsFilePathManager.AppSettingsFilePath);
            File.WriteAllText(appSettingsFilePathManager.AppSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            JObject azure = JObject.FromObject(info);
            ((JArray)jObject["RDBMS"]["nocloud"]).Add(azure);
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePathManager.AppSettingsFilePath, updatedJson);
        }

        public void AddNewConnection(Connections connectionInfo, string dbname)
        {
            var json = File.ReadAllText(appSettingsFilePathManager.AppSettingsFilePath);
            File.WriteAllText(appSettingsFilePathManager.AppSettingsFilePath_BCK, json);
            var jObject = JObject.Parse(json);
            JObject newConnection = JObject.FromObject(connectionInfo);
            JArray azureArray = (JArray)jObject["RDBMS"]["nocloud"];
            JObject sqlObject = azureArray.Children<JObject>().FirstOrDefault(o => o["name"] != null && o["name"].ToString().ToLower() == dbname);
            JArray connectionsArray = (JArray)sqlObject["connections"];
            connectionsArray.Add(newConnection);
            string updatedJson = jObject.ToString();
            File.WriteAllText(appSettingsFilePathManager.AppSettingsFilePath, updatedJson);
        }

        public List<RDBMSInfo> GetListDataBase()
        {
            var json = File.ReadAllText(appSettingsFilePathManager.AppSettingsFilePath);
            var jObject = JObject.Parse(json);
            var azureRDBMS = jObject["RDBMS"]["nocloud"];
            List<RDBMSInfo> rdbmsInfoList = JsonConvert.DeserializeObject<List<RDBMSInfo>>(azureRDBMS.ToString());
            return rdbmsInfoList;
        }

        public List<Connections> GetListOfConnections(string dbname)
        {
            var json = File.ReadAllText(appSettingsFilePathManager.AppSettingsFilePath);
            var jObject = JObject.Parse(json);
            var azureRDBMS = jObject["RDBMS"]["nocloud"];
            JObject sqlObject = azureRDBMS.Children<JObject>().FirstOrDefault(o => o["name"] != null && o["name"].ToString().ToLower() == dbname.ToLower());
            List<Connections> connections = JsonConvert.DeserializeObject<List<Connections>>(sqlObject["connections"].ToString());
            return connections;
        }
    }
}
