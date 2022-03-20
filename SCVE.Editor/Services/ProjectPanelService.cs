using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class ProjectPanelService : IService, IFileDropReceiver
    {
        public bool HasSelectedLocation { get; private set; }

        public string CurrentLocation { get; private set; }

        public IReadOnlyList<SequenceAsset> Sequences { get; private set; }
        public IReadOnlyList<ImageAsset> Images { get; private set; }

        public IReadOnlyList<FolderAsset> Folders { get; private set; }

        private EditingService _editingService;

        public ProjectPanelService(EditingService editingService)
        {
            _editingService = editingService;
        }

        /// <summary>
        /// Changes the visible location of ProjectPanel. If null, clears the visible assets
        /// </summary>
        public void ChangeLocation(string location)
        {
            if (location is null)
            {
                HasSelectedLocation = false;
                Sequences = null;
                Images = null;
                Folders = null;
                CurrentLocation = null;
            }
            else
            {
                CurrentLocation = location;
                HasSelectedLocation = true;
                RescanCurrentLocation();
            }

            Console.WriteLine($"Project Panel switched to Location: \"{location}\"");
        }

        public void RescanCurrentLocation()
        {
            Sequences = _editingService.OpenedProject.Sequences.Where(s => s.Location == CurrentLocation).ToList();
            Images = _editingService.OpenedProject.Images.Where(i => i.Location == CurrentLocation).ToList();
            Folders = _editingService.OpenedProject.Folders.Where(i => i.Location == CurrentLocation).ToList();
        }

        public void LevelUp()
        {
            ChangeLocation(CurrentLocation.Substring(0, CurrentLocation.LastIndexOf('/') + 1));
        }

        public void OnFileDrop(string[] paths)
        {
            if (_editingService.OpenedProject is null)
            {
                return;
            }
            
            if (Path.GetExtension(paths[0]) == ".scveproject")
            {
                DropScveProject(paths[0]);
            }

            DropImages(paths);
        }

        private void DropScveProject(string path)
        {
            throw new NotImplementedException("SCVE project must be an actual Asset." +
                                              "So that we can pass it a link to the original project.");
        }

        private void DropImages(string[] paths)
        {
            foreach (var path in paths)
            {
                var name = Path.GetFileName(path);
                var relativePath = Path.GetRelativePath(Environment.CurrentDirectory, path);
                _editingService.OpenedProject.AddImage(
                    ImageAsset.CreateNew(name, CurrentLocation, Image.CreateNew(relativePath))
                );
            }

            RescanCurrentLocation();
        }
    }
}