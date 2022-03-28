using System;
using System.IO;
using System.Linq;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.ProjectStructure;
using SCVE.Editor.Services.Loaders;

namespace SCVE.Editor.Services
{
    public class FileDropReceiverService : IService, IFileDropReceiver
    {
        private ProjectPanelService _projectPanelService;
        private ProjectLoaderService _projectLoaderService;
        private ImageLoaderService _imageLoaderService;
        private EditingService _editingService;
        private RecentsService _recentsService;
        private PreviewService _previewService;

        public FileDropReceiverService(ProjectPanelService projectPanelService, ProjectLoaderService projectLoaderService, ImageLoaderService imageLoaderService, EditingService editingService, RecentsService recentsService, PreviewService previewService)
        {
            _projectPanelService = projectPanelService;
            _projectLoaderService = projectLoaderService;
            _imageLoaderService = imageLoaderService;
            _editingService = editingService;
            _recentsService = recentsService;
            _previewService = previewService;
        }

        public void OnFileDrop(string[] paths)
        {
            var projects = paths.Where(p => ProjectLoaderService.IsSupportedExtension(Path.GetExtension(p))).ToList();

            switch (projects.Count)
            {
                case > 1:
                    // TODO: Handle multiple imported projects (via selector modal)
                    Console.WriteLine("Unable to import more than 1 project at a time");
                    break;
                case 1:
                {
                    if (_editingService.OpenedProject is not null)
                    {
                        Console.WriteLine("Loading new project, while there is a loaded one");
                    }

                    var path = projects[0];

                    var project = _projectLoaderService.Load(path);

                    EditorApp.Late("open project", () =>
                    {
                        _editingService.SetOpenedProject(project, path);
                        _recentsService.NoticeOpen(path);
                        _projectPanelService.ChangeLocation("/");
                        _previewService.SyncVisiblePreview();

                        Console.WriteLine($"Loaded project {project.Title}");
                    });

                    break;
                }
            }

            var images = paths.Where(p => ImageLoaderService.IsSupportedExtension(Path.GetExtension(p))).ToList();
            if (images.Count > 0)
            {
                if (_editingService.OpenedProject is null)
                {
                    Console.WriteLine("Can't import image, no project is opened");
                }
                else
                {
                    foreach (var path in images)
                    {
                        var image = _imageLoaderService.Load(path);

                        var fileName = Path.GetFileName(path);
                        var imageAsset = ImageAsset.CreateNew(
                            name: fileName,
                            location: _projectPanelService.CurrentLocation,
                            content: image
                        );

                        EditorApp.Late("add image", () =>
                        {
                            _editingService.OpenedProject.AddImage(imageAsset);

                            _projectPanelService.RescanCurrentLocation();

                            Console.WriteLine($"Loaded image {fileName}");
                        });
                    }
                }
            }
        }
    }
}