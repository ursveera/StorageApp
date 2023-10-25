namespace StorageApp.FileManager
{
    public class AppSettingsFilePathManager
    {
        private static readonly Lazy<AppSettingsFilePathManager> instance =
        new Lazy<AppSettingsFilePathManager>(() => new AppSettingsFilePathManager());

        public string AppSettingsFilePath { get; }
        public string AppSettingsFilePath_BCK { get; }

        public static AppSettingsFilePathManager Instance => instance.Value;
        private AppSettingsFilePathManager()
        {
            AppSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            AppSettingsFilePath_BCK = Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK", Guid.NewGuid().ToString() + "_appsettings_bck.json");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "SettingBCK"));
            }
        }
    }
}
