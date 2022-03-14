using System;
using System.Collections.Generic;
using System.Numerics;
using SCVE.Editor.Abstractions;
using System.IO;
using Silk.NET.Vulkan;

namespace SCVE.Editor.Services
{
    public class SettingsService : IService
    {
        public Settings SettingsInstance => _settings;
        private Settings _settings;

        public void TryLoad()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "settings.json");
            if (File.Exists(path))
            {
                _settings = Utils.ReadJson<Settings>(path);

                Console.WriteLine("Loaded settings");
            }
            else
            {
                _settings = new();

                Console.WriteLine("Settings not found");
            }
        }

        public void TrySave()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "settings.json");

            Utils.WriteJson(_settings, path);

            Console.WriteLine("Saved settings.");
        }
    }
}