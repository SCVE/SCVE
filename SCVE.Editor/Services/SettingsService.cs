using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;
using SCVE.Editor.Abstractions;
using System.IO;
using Silk.NET.Vulkan;

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
                Settings.SetFrom(freshSettings);

                Console.WriteLine("Loaded settings");
            }
            else
            {
                Settings.SetFromDefault();

                Console.WriteLine("Settings not found");
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
            Settings.SetFrom(draftSettings);
            TrySave();
        }
    }
}