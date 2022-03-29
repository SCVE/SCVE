using SCVE.Editor.Abstractions;
using SCVE.Editor.Services;

namespace SCVE.Editor.Late
{
    public class LateTaskVisitor : IService
    {
        public EditingService EditingService { get; set; }

        public RecentsService RecentsService { get; set; }

        public ProjectPanelService ProjectPanelService { get; set; }

        public PreviewService PreviewService { get; set; }

        public SettingsService SettingsService { get; set; }

        public LateTaskVisitor(EditingService editingService, RecentsService recentsService, ProjectPanelService projectPanelService, PreviewService previewService, SettingsService settingsService)
        {
            EditingService = editingService;
            RecentsService = recentsService;
            ProjectPanelService = projectPanelService;
            PreviewService = previewService;
            SettingsService = settingsService;
        }
    }
}