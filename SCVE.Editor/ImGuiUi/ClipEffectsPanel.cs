using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using SCVE.Editor.Effects;

namespace SCVE.Editor.ImGuiUi
{
    public class ClipEffectsPanel : IImGuiRenderable
    {
        private List<Type> AllKnownEffects;
        private string[] AllKnownEffectsLabels;

        public ClipEffectsPanel()
        {
            AllKnownEffects       = Assembly.GetExecutingAssembly().ExportedTypes.Where(t => t.IsAssignableTo(typeof(IEffect)) && !t.IsInterface).ToList();
            AllKnownEffectsLabels = AllKnownEffects.Select(t => t.Name).ToArray();
        }

        private bool _addEffectExpanded;

        public void OnImGuiRender()
        {
            if (ImGui.Begin("Clip Effects"))
            {
                var clip = EditorApp.Instance.SelectedClip;
                if (clip is null)
                {
                    ImGui.Text("No Clip Is Selected");
                    ImGui.End();
                    return;
                }

                for (var i = 0; i < clip.Effects.Count; i++)
                {
                    if (ImGui.TreeNodeEx($"{clip.Effects[i].VisibleName}##clip-effect-{i}", ImGuiTreeNodeFlags.SpanFullWidth))
                    {
                        clip.Effects[i].OnImGuiRender();
                        ImGui.TreePop();
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
                            }
                        }

                        ImGui.EndPopup();
                    }
                }
            }
        }
    }
}