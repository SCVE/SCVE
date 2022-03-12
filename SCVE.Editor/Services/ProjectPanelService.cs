using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SCVE.Editor.Editing.ProjectStructure;

namespace SCVE.Editor.Services
{
    public class ProjectPanelService : IService
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
                Sequences = _editingService.OpenedProject.Sequences.Where(s => s.Location == location).ToList();
                Images = _editingService.OpenedProject.Images.Where(i => i.Location == location).ToList();
                Folders = _editingService.OpenedProject.Folders.Where(i => i.Location == location).ToList();
                CurrentLocation = location;
                HasSelectedLocation = true;
            }

            Console.WriteLine($"Project Panel switched to Location: \"{location}\"");
        }

        public void LevelUp()
        {
            ChangeLocation(CurrentLocation.Substring(0, CurrentLocation.LastIndexOf('/') + 1));
        }

        public void OnUpdate()
        {
        }
    }
}