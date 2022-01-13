using System.IO;
using System.IO.Compression;
using System.Text.Json;
using SCVE.Editor.ProjectStructure;
using SCVE.Engine.Core.Misc;

namespace SCVE.Editor
{
    public static class Utils
    {
        public static bool IsDirectory(this FileSystemInfo info)
        {
            // get the file attributes for file or directory
            FileAttributes attr = info.Attributes;

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return true;
            else
                return false;
        }
        
        public static bool IsDirectoryPath(this string path)
        {
            return Directory.Exists(path);
        }
        
        
        public static void CreateDummyProject(string name, string path)
        {
            if (path.IsDirectoryPath())
            {
                var       projectPath       = Path.Combine(path, name + ".scve");
                using var zipStream         = new FileStream(projectPath, FileMode.CreateNew);
                using var zipProjectArchive = new ZipArchive(zipStream, ZipArchiveMode.Update);
                var       metaEntry         = zipProjectArchive.CreateEntry("project.meta");
                zipProjectArchive.CreateEntry("assets\\");

                using var metaWriter = new StreamWriter(metaEntry.Open());

                var projectMetaFileData = new ProjectMetaFileData("Test Project ABC", "SCVE PROJECT", "1.0");
                var metaFileContent     = JsonSerializer.Serialize(projectMetaFileData);
                metaWriter.WriteLine(metaFileContent);
                AppendTextAsset(zipProjectArchive);
                AppendImageAsset(zipProjectArchive);
                AppendMP3Asset(zipProjectArchive);
            }
            else
            {
                throw new ScveException("Path was not a directory");
            }
        }

        private static void AppendImageAsset(ZipArchive zipProjectArchive)
        {
            var       assetEntry  = zipProjectArchive.CreateEntry("assets\\images\\image.scveasset");
            using var assetWriter = new StreamWriter(assetEntry.Open());

            var assetFileData    = new ProjectAssetFileData("IMAGE", "C:\\Projects\\CSharp\\SCVE\\testdata\\runner2.png");
            var assetFileContent = JsonSerializer.Serialize(assetFileData);
            assetWriter.WriteLine(assetFileContent);
        }

        private static void AppendMP3Asset(ZipArchive zipProjectArchive)
        {
            var       assetEntry  = zipProjectArchive.CreateEntry("assets\\audio.scveasset");
            using var assetWriter = new StreamWriter(assetEntry.Open());

            var assetFileData    = new ProjectAssetFileData("MP3", "C:\\Projects\\CSharp\\SCVE\\testdata\\rukoblud.mp3");
            var assetFileContent = JsonSerializer.Serialize(assetFileData);
            assetWriter.WriteLine(assetFileContent);
        }

        private static void AppendTextAsset(ZipArchive zipProjectArchive)
        {
            var       assetEntry  = zipProjectArchive.CreateEntry("assets\\folder\\readme.scveasset");
            using var assetWriter = new StreamWriter(assetEntry.Open());

            var assetFileData    = new ProjectAssetFileData("TEXT", "C:\\Projects\\CSharp\\SCVE\\testdata\\readme.txt");
            var assetFileContent = JsonSerializer.Serialize(assetFileData);
            assetWriter.WriteLine(assetFileContent);
        }
    }
}