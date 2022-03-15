using System;
using SCVE.Editor.Abstractions;
using System.IO;

namespace SCVE.Editor.Services
{
    public class SettingsService : IService
    {
        public void TryLoad()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "settings.json");
            if (File.Exists(path))
            {
                var freshSettings = Utils.ReadJson<Settings>(path);

                TryMigrateSettings(freshSettings);

                Settings.SetFrom(freshSettings);

                Console.WriteLine("Loaded settings");
            }
            else
            {
                Settings.SetFrom(Settings.Default);

                Console.WriteLine("Settings not found");
            }
        }

        private void TryMigrateSettings(Settings settings)
        {
            switch (settings.Version)
            {
                case 0:
                    MigrateSettingsToV1(settings);
                    break;
                case 1: break;
            }
        }

        public void TrySave()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "settings.json");

            Utils.WriteJson(Settings.Instance, path);

            Console.WriteLine("Saved settings.");
        }

        public void ApplySettings(Settings draftSettings)
        {
            Settings.SetFromCopy(draftSettings);
            TrySave();
        }

        public void MigrateSettingsToV1(Settings settings)
        {
            settings.Version = 1;
            settings.ClipPadding = Settings.Default.ClipPadding;
            settings.ClipRounding = Settings.Default.ClipRounding;

            Console.WriteLine("Migrated settings to V1");
        }
    }
}