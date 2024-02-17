using System.Runtime.InteropServices;
using ImGuiNET;
using SCVE.Editor.Core;

namespace SCVE.Editor.ImGuiUi.Panels
{
    public class ProjectCreationPanel : ImGuiModalPanel
    {
        protected override string ImGuiId => "Create project##create-project";
        
        private string _title = "";

        private List<string> _errors = new();

        public override void Open()
        {
            base.Open();
            _errors.Clear();
        }

        protected override void OnImGuiRenderContent()
        {
            if (ImGui.InputTextWithHint(
                    "Project title",
                    "specify any title for the project",
                    ref _title,
                    256
                ))
            {
            }
            
            ImGui.PushStyleColor(ImGuiCol.Text, 0xFF0000FF);
            foreach (var error in _errors)
            {
                ImGui.Text(error);
            }
            ImGui.PopStyleColor();

            if (ImGui.Button("Create project"))
            {
                bool canCreate = true;
                _errors.Clear();
                if (_title.IsNullOrEmpty())
                {
                    _errors.Add("Title should not be empty");
                    canCreate = false;
                }

                if (canCreate)
                {
                    var project = new Project();
                    EditorApp.Instance.OpenProject(project);
                    
                    ImGui.CloseCurrentPopup();
                }
            }
        }
    }
}