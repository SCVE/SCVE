using System;
using ImGuiNET;

namespace SCVE.Editor.ImGuiUi
{
    public class ImGuiEnumContextMenu<T> where T : struct, Enum
    {
        private T[] _values;
        private int _currentIndex;
        private string _name;

        public ImGuiEnumContextMenu(int currentIndex, string name)
        {
            _values = Enum.GetValues<T>();
            _currentIndex = currentIndex;
            _name = name;
        }

        public T OnImGuiRender()
        {
            ImGui.SetNextItemWidth(300);
            if (ImGui.BeginCombo(_name, _values[_currentIndex].ToString()))
            {
                for (var i = 0; i < _values.Length; i++)
                {
                    if (ImGui.Selectable(_values[i].ToString(), _currentIndex == i))
                    {
                        _currentIndex = i;
                        break;
                    }
                }

                ImGui.EndCombo();
            }

            return _values[_currentIndex];
        }
    }
}