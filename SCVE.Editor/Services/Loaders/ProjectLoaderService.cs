using System;
using System.IO;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services.Loaders
{
    public class ProjectLoaderService : IService
    {
        private const string ProjectFileExtension = ".scveproject";

        public static bool IsSupportedExtension(string extension)
        {
            if (extension is null)
            {
                return false;
            }

            return extension == ProjectFileExtension;
        }

        public bool TryLoad(string path, out VideoProject videoProject)
        {
            if (Path.GetExtension(path) == ProjectFileExtension)
            {
                videoProject = Utils.ReadJson<VideoProject>(path);

                Console.WriteLine($"Loaded project: {videoProject.Title}");
                return true;
            }
            else
            {
                Console.WriteLine($"Unknown project file type selected: {Path.GetExtension(path)}");

                videoProject = null;
                return false;
            }
        }

        public VideoProject Load(string path)
        {
            return Utils.ReadJson<VideoProject>(path);
        }
    }
}