using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class RecentsService : IService
    {
        public IReadOnlyList<string> Recents => _recents;
        private List<string> _recents;

        public void OnUpdate()
        {
        }

        public void TryLoad()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "recents.json");
            if (File.Exists(path))
            {
                var jsonContent = File.ReadAllText(path);

                var recents = JsonSerializer.Deserialize<List<string>>(jsonContent,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                _recents = recents!;

                _recents = _recents.Where(File.Exists).ToList();

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

            var jsonContent = JsonSerializer.Serialize(Recents,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            File.WriteAllText(path, jsonContent);
            
            Console.WriteLine("Saved Recents");
        }

        public void NoticeOpenRecent(string recentPath)
        {
            _recents.Remove(recentPath);
            _recents.Insert(0, recentPath);
        }

        public void NoticeOpenNew(string path)
        {
            _recents.Insert(0, path);
        }
    }
}