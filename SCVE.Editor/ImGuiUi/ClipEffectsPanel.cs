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
        private IList<Type> AllKnownEffects;

        private EffectImGuiRenderer _renderer;

        private EditingService _editingService;
        private PreviewService _previewService;

        public ClipEffectsPanel(EditingService editingService, PreviewService previewService)
        {
            _editingService = editingService;
            _previewService = previewService;

            AllKnownEffects = Utils.GetAssignableTypes<EffectBase>();

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
                if (ImGui.TreeNodeEx($"{clip.Effects[i].GetType().Name}##clip-effect-{i}", ImGuiTreeNodeFlags.SpanFullWidth))
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
                ImGui.SetNextWindowPos(ImGui.GetWindowPos() + ImGui.GetCursorPos());
                if (ImGui.BeginPopupModal("##add-effect-contextmenu", ref _addEffectExpanded, ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar))
                {
                    for (var i = 0; i < AllKnownEffects.Count; i++)
                    {
                        if (ImGui.Selectable(AllKnownEffects[i].Name))
                        {
                            _addEffectExpanded = false;
                            var effect = Activator.CreateInstance(AllKnownEffects[i]) as EffectBase;

                            effect!.Updated += OnEffectUpdated;
                            clip.Effects.Add(effect);
                            _previewService.InvalidateRange(clip.StartFrame, clip.FrameLength);
                        }
                    }

                    if (ImGui.Button("Cancel"))
                    {
                        _addEffectExpanded = false;
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