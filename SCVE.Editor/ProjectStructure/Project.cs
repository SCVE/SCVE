using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using SCVE.Engine.Core.Misc;

namespace SCVE.Editor.ProjectStructure
{
    public class Project : IDisposable
    {
        private Stream _archiveStream;
        private ZipArchive _archive;

        public string Name { get; set; }

        public string Type { get; set; }

        public string Version { get; set; }

        public readonly ProjectAssetFolder RootFolder;

        public Project(string path) : this(new FileStream(path, FileMode.Open))
        {
        }

        public Project(Stream stream)
        {
            _archiveStream = stream;

            _archive   = new ZipArchive(_archiveStream, ZipArchiveMode.Read);
            RootFolder = new(Guid.Empty, "", "");
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
            string type               = "";
            string fileSystemFullPath = "";
            Guid   guid = Guid.Empty;
            if (entry.Name.EndsWith(".scveasset"))
            {
                var assetContent         = ReadAssetContent(entry);
                var projectAssetFileData = JsonSerializer.Deserialize<ProjectAssetFileData>(assetContent);

                type               = projectAssetFileData.Type;
                fileSystemFullPath = projectAssetFileData.FileSystemPath;
                guid               = projectAssetFileData.Guid;
            }

            if (entry.FullName.Contains(Path.DirectorySeparatorChar))
            {
                var entryPath = entry.FullName;
                entryPath = entryPath.Substring(0, entryPath.LastIndexOf(Path.DirectorySeparatorChar));
                if (entry.Name == "")
                {
                    RootFolder.AppendEmptyFolder(entryPath, entry.FullName);
                }
                else
                {
                    ProjectAsset asset;
                    if (type == "IMAGE")
                    {
                        asset = new ImageAsset(guid, entry.Name, entry.FullName, fileSystemFullPath, type);
                    }
                    else
                    {
                        asset = new ProjectAsset(guid, entry.Name, entry.FullName, fileSystemFullPath, type);
                    }
                    RootFolder.AppendAsset(entryPath, asset);
                }
            }
            else
            {
                ProjectAsset asset;
                if (type == "IMAGE")
                {
                    asset = new ImageAsset(guid, entry.Name, entry.FullName, fileSystemFullPath, type);
                }
                else
                {
                    asset = new ProjectAsset(guid, entry.Name, entry.FullName, fileSystemFullPath, type);
                }
                RootFolder.AppendAsset("", asset);
            }
        }

        private string ReadAssetContent(ZipArchiveEntry entry)
        {
            using var entryStream = entry.Open();
            using var entryReader = new StreamReader(entryStream);
            return entryReader.ReadToEnd();
        }

        private void ReadMetaFile()
        {
            var metaFileContent = GetMetaFileContent();

            Name    = metaFileContent.Name;
            Type    = metaFileContent.Type;
            Version = metaFileContent.Version;
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

            var projectMetaFileData = JsonSerializer.Deserialize<ProjectMetaFileData>(metaFileReader.ReadToEnd());

            if (projectMetaFileData.Type == "SCVE PROJECT")
            {
                return true;
            }

            return false;
        }

        public static Project LoadFrom(string path)
        {
            return new Project(path);
        }

        public ProjectMetaFileData GetMetaFileContent()
        {
            var       metaFileEntry       = _archive.GetEntry("project.meta");
            using var metaFileStream      = metaFileEntry?.Open() ?? throw new ScveException("project.meta not found ");
            using var metaFileReader      = new StreamReader(metaFileStream);
            var       projectMetaFileData = JsonSerializer.Deserialize<ProjectMetaFileData>(metaFileReader.ReadToEnd());
            return projectMetaFileData;
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