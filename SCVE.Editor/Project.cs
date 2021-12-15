using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using SCVE.Engine.Core.Misc;

namespace SCVE.Editor
{
    public class Project : ProjectAssetFolder, IDisposable
    {
        private Stream _archiveStream;
        private ZipArchive _archive;

        public string Name { get; set; }

        public Project(string path) : this(new FileStream(path, FileMode.Open))
        {
        }

        public Project(Stream stream) : base("")
        {
            _archiveStream = stream;

            _archive = new ZipArchive(_archiveStream, ZipArchiveMode.Read);
            ReadMetaFile();
            ReadAssetsFolder();
        }

        private void ReadAssetsFolder()
        {
            foreach (var entry in _archive.Entries)
            {
                AppendAssetFromArchive(entry);
            }
        }

        private void AppendAssetFromArchive(ZipArchiveEntry entry)
        {
            if (entry.FullName.Contains("\\"))
            {
                var entryPath = entry.FullName;
                entryPath = entryPath.Substring(0, entryPath.LastIndexOf(Path.DirectorySeparatorChar));
                if (entry.Name == "")
                {
                    AppendEmptyFolder(entryPath);
                }
                else
                {
                    AppendAsset(entryPath, entry.Name);
                }
            }
            else
            {
                AppendAsset("", entry.Name);
            }
        }

        private void ReadMetaFile()
        {
            var metaFileContent = GetMetaFileContent();

            string name = metaFileContent.Split('\n')[1];
            Name = name;
        }

        public static void Create(string name, string path)
        {
            if (path.IsDirectoryPath())
            {
                var       projectPath       = Path.Combine(path, name + ".scve");
                using var zipStream         = new FileStream(projectPath, FileMode.CreateNew);
                using var zipProjectArchive = new ZipArchive(zipStream, ZipArchiveMode.Update);
                var       metaEntry         = zipProjectArchive.CreateEntry("project.meta");
                zipProjectArchive.CreateEntry("assets\\");
                using var metaWriter = new StreamWriter(metaEntry.Open());
                metaWriter.WriteLine("SCVE 1.0 Project");
                metaWriter.WriteLine(name);

                var       addEntry  = zipProjectArchive.CreateEntry("assets\\folder\\readme.txt");
                using var addWriter = new StreamWriter(addEntry.Open());
                addWriter.WriteLine("I am awesome");
            }
            else
            {
                throw new ScveException("Path was not a directory");
            }
        }

        public static bool PathIsProject(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            using var zipStream         = new FileStream(path, FileMode.Open);
            using var zipProjectArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);
            var       metaFileEntry     = zipProjectArchive.GetEntry("project.meta");
            using var metaFileStream    = metaFileEntry.Open();
            using var metaFileReader    = new StreamReader(metaFileStream);

            var firstLine = metaFileReader.ReadLine();
            if (firstLine == "SCVE 1.0 Project")
            {
                return true;
            }

            return false;
        }

        public static Project LoadFrom(string path)
        {
            return new Project(path);
        }

        public string GetMetaFileContent()
        {
            var       metaFileEntry  = _archive.GetEntry("project.meta");
            using var metaFileStream = metaFileEntry.Open();
            using var metaFileReader = new StreamReader(metaFileStream);
            return metaFileReader.ReadToEnd();
        }

        public void Dispose()
        {
            _archive?.Dispose();
            _archiveStream?.Dispose();
        }

        public static void Delete(string name, string path)
        {
            if (path.IsDirectoryPath())
            {
                var projectPath = Path.Combine(path, name + ".scve");
                if (File.Exists(projectPath))
                {
                    File.Delete(projectPath);
                }
            }
            else
            {
                throw new ScveException("Path was not a directory");
            }
        }
    }
}