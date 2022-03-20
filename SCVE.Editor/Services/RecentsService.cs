using System;
using System.Collections.Generic;
using System.IO;
using SCVE.Editor.Abstractions;

namespace SCVE.Editor.Services
{
    public class RecentsService : IService
    {
        public IReadOnlyList<string> Recents => _recents;
        private List<string> _recents;

        public void TryLoad()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "recents.json");
            if (File.Exists(path))
            {
                _recents = Utils.ReadJson<List<string>>(path);

                Console.WriteLine("Loaded recents");
            }
            else
            {
                _recents = new List<string>();

                Console.WriteLine("Recents not found");
            }
        }

        public void TrySave()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "recents.json");

            Utils.WriteJson(Recents, path);

            Console.WriteLine("Saved Recents");
        }

        public void NoticeOpen(string recentPath)
        {
            _recents.Remove(recentPath);
            _recents.Insert(0, recentPath);
        }
    }
}