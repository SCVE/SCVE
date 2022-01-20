using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using SCVE.Editor.Effects;
using SCVE.Editor.Modules;
using Silk.NET.GLFW;

namespace SCVE.Editor.ImGuiUi
{
    public class ClipEffectsPanel : IImGuiRenderable
    {
        private List<Type> AllKnownEffects;
        private string[] AllKnownEffectsLabels;

        private EditingModule _editingModule;
        private PreviewModule _previewModule;

        public ClipEffectsPanel()
        {
            AllKnownEffects       = Assembly.GetExecutingAssembly().ExportedTypes.Where(t => t.IsAssignableTo(typeof(IEffect)) && !t.IsInterface).ToList();
            AllKnownEffectsLabels = AllKnownEffects.Select(t => EffectsVisibleNames.Names[t]).ToArray();
            _editingModule        = EditorApp.Modules.Get<EditingModule>();
            _previewModule        = EditorApp.Modules.Get<PreviewModule>();
        }

        private bool _addEffectExpanded;

        private int _lastSelectedEffect = -1;

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Clip Effects"))
            {
                var clip = _editingModule.SelectedClip;
                if (clip is null)
                {
                    ImGui.Text("No Clip Is Selected");
                    ImGui.End();
                    return;
                }

                for (var i = 0; i < clip.Effects.Count; i++)
                {
                    if (ImGui.TreeNodeEx($"{EffectsVisibleNames.Names[clip.Effects[i].GetType()]}##clip-effect-{i}", ImGuiTreeNodeFlags.SpanFullWidth))
                    {
                        clip.Effects[i].OnImGuiRender();
                        ImGui.TreePop();
                    }

                    if (ImGui.IsItemClicked())
                    {
                        _lastSelectedEffect = i;
                    }
                }

                if (!_addEffectExpanded)
                {
                    if (ImGui.Button("+"))
                    {
                        _addEffectExpanded = !_addEffectExpanded;
                        ImGui.OpenPopup("##add-effect-contextmenu");
                    }
                }
                else
                {
                    ImGui.SetNextWindowSize(new Vector2(200, 200));
                    ImGui.SetNextWindowPos(ImGui.GetWindowPos());
                    if (ImGui.BeginPopupModal("##add-effect-contextmenu", ref _addEffectExpanded, ImGuiWindowFlags.NoResize))
                    {
                        for (var i = 0; i < AllKnownEffectsLabels.Length; i++)
                        {
                            if (ImGui.Selectable(AllKnownEffectsLabels[i]))
                            {
                                _addEffectExpanded = false;
                                clip.Effects.Add(Activator.CreateInstance(AllKnownEffects[i]) as IEffect);
                                _previewModule.InvalidateSampledFrame(_editingModule.OpenedSequence.CursorTimeFrame);
                            }
                        }

                        ImGui.EndPopup();
                    }
                }

                if (ImGui.IsKeyPressed((int)Keys.Delete))
                {
                    if (_lastSelectedEffect != -1)
                    {
                        clip.Effects.RemoveAt(_lastSelectedEffect);
                        _lastSelectedEffect = -1;
                        _previewModule.InvalidateSampledFrame(_editingModule.OpenedSequence.CursorTimeFrame);
                    }
                }
            }
        }
    }
}