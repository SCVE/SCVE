using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using SCVE.Editor.Editing.Effects;
using SCVE.Editor.Services;
using Silk.NET.GLFW;

namespace SCVE.Editor.ImGuiUi
{
    public class ClipEffectsPanel : IImGuiRenderable
    {
        private List<Type> AllKnownEffects;
        private string[] AllKnownEffectsLabels;

        private EffectImGuiRenderer _renderer;

        private EditingService _editingService;
        private PreviewService _previewService;

        public ClipEffectsPanel(EditingService editingService, PreviewService previewService)
        {
            _editingService = editingService;
            _previewService = previewService;
            var referencedAssemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            AllKnownEffects = referencedAssemblyNames.SelectMany(name => Assembly.Load(name).ExportedTypes.Where(t => t.IsAssignableTo(typeof(EffectBase)) && !t.IsAbstract)).ToList();
            AllKnownEffectsLabels = AllKnownEffects.Select(t => EffectsVisibleNames.Names[t]).ToArray();

            _renderer = new EffectImGuiRenderer();
        }

        private bool _addEffectExpanded;

        private int _lastSelectedEffect = -1;

        public void OnImGuiRender()
        {
            if (!ImGui.Begin("Clip Effects"))
            {
                goto END;
            }

            var clip = _editingService.SelectedClip;
            if (clip is null)
            {
                ImGui.Text("No Clip Is Selected");
                goto END;
            }

            for (var i = 0; i < clip.Effects.Count; i++)
            {
                if (ImGui.TreeNodeEx($"{EffectsVisibleNames.Names[clip.Effects[i].GetType()]}##clip-effect-{i}", ImGuiTreeNodeFlags.SpanFullWidth))
                {
                    _renderer.Visit(clip.Effects[i]);
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
                            var effect = Activator.CreateInstance(AllKnownEffects[i]) as EffectBase;
                            
                            effect.Updated += OnEffectUpdated;
                            clip.Effects.Add(effect);
                            _previewService.InvalidateRange(clip.StartFrame, clip.FrameLength);
                        }
                    }

                    ImGui.EndPopup();
                }
            }

            if (ImGui.IsKeyPressed((int) Keys.Delete))
            {
                if (_lastSelectedEffect != -1)
                {
                    // clip.Effects[_lastSelectedEffect].Updated -= OnEffectOnUpdated;
                    // clip.RemoveEffect(_lastSelectedEffect);
                    _lastSelectedEffect = -1;
                    _previewService.InvalidateRange(clip.StartFrame, clip.FrameLength);
                }
            }

            END:
            ImGui.End();
        }


        void OnEffectUpdated() => _previewService.InvalidateRange(_editingService.SelectedClip.StartFrame, _editingService.SelectedClip.FrameLength);
    }
}