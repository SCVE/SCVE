using System.Collections.Generic;
using ImGuiNET;
using SCVE.Editor.Abstractions;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiSelectableContextMenu<T> where T : IVisible
    {
        private IReadOnlyList<T> _elements;
        private int _currentIndex;
        private string _name;

        public ImGuiSelectableContextMenu(IReadOnlyList<T> elements, int currentIndex, string name)
        {
            _elements = elements;
            _currentIndex = currentIndex;
            _name = name;
        }

        public int OnImGuiRender()
        {
            ImGui.SetNextItemWidth(300);
            if (ImGui.BeginCombo(_name, _elements[_currentIndex].VisibleTitle))
            {
                for (var i = 0; i < _elements.Count; i++)
                {
                    if (ImGui.Selectable(_elements[i].VisibleTitle, _currentIndex == i))
                    {
                        _currentIndex = i;
                        break;
                    }
                }

                ImGui.EndCombo();
            }

            return _currentIndex;
        }
    }
}