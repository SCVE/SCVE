using System;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Text.Json;
using SCVE.Editor.Editing;
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

        public static Sequence CreateTestingSequence()
        {
            var sequence = Sequence.CreateNew(30, new Vector2(1920, 1080));
            sequence.FrameLength = 100;
            sequence.AddTrack(Track.CreateNew());
            sequence.AddTrack(Track.CreateNew());
            sequence.AddTrack(Track.CreateNew());

            sequence.Tracks[0].AddClip(EmptyClip.CreateNew(0, 10));
            sequence.Tracks[0].AddClip(EmptyClip.CreateNew(30, 30));
            sequence.Tracks[0].AddClip(EmptyClip.CreateNew(60, 30));

            sequence.Tracks[1].AddClip(EmptyClip.CreateNew(10, 10));
            sequence.Tracks[1].AddClip(EmptyClip.CreateNew(20, 10));
            sequence.Tracks[1].AddClip(EmptyClip.CreateNew(40, 15));

            sequence.Tracks[2].AddClip(ImageClip.CreateNew(10,30, Guid.Parse("53d08676-4b40-4efe-bab7-2588dc697e25")));

            return sequence;
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

            var assetFileData    = new ProjectAssetFileData(Guid.Parse("53d08676-4b40-4efe-bab7-2588dc697e25"), "IMAGE", "C:\\Projects\\CSharp\\SCVE\\testdata\\Jessei.png");
            var assetFileContent = JsonSerializer.Serialize(assetFileData);
            assetWriter.WriteLine(assetFileContent);
        }

        private static void AppendMP3Asset(ZipArchive zipProjectArchive)
        {
            var       assetEntry  = zipProjectArchive.CreateEntry("assets\\audio.scveasset");
            using var assetWriter = new StreamWriter(assetEntry.Open());

            var assetFileData    = new ProjectAssetFileData(Guid.Parse("1cff5fe5-e8a4-4e74-828f-32c947ad9e66"), "MP3", "C:\\Projects\\CSharp\\SCVE\\testdata\\rukoblud.mp3");
            var assetFileContent = JsonSerializer.Serialize(assetFileData);
            assetWriter.WriteLine(assetFileContent);
        }

        private static void AppendTextAsset(ZipArchive zipProjectArchive)
        {
            var       assetEntry  = zipProjectArchive.CreateEntry("assets\\folder\\readme.scveasset");
            using var assetWriter = new StreamWriter(assetEntry.Open());

            var assetFileData    = new ProjectAssetFileData(Guid.Parse("bd32eda7-9027-4f0b-9e81-ba7f763507c3"), "TEXT", "C:\\Projects\\CSharp\\SCVE\\testdata\\readme.txt");
            var assetFileContent = JsonSerializer.Serialize(assetFileData);
            assetWriter.WriteLine(assetFileContent);
        }
    }
}