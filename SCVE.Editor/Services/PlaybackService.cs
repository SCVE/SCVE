using System;
using SCVE.Editor.Abstractions;
using Silk.NET.Input;

namespace SCVE.Editor.Services
{
    public class PlaybackService : IService, IKeyPressReceiver
    {
        public bool IsPlaying { get; private set; }

        private float _timeAccumulator = 0f;

        private int _startFrame = 0;

        private EditingService _editingService;
        private PreviewService _previewService;

        public PlaybackService(
            EditingService editingService,
            PreviewService previewService)
        {
            _editingService = editingService;
            _previewService = previewService;
        }

        public void Play()
        {
            if (_editingService.OpenedSequence is null)
            {
                Console.WriteLine("Can't play, no sequence is opened");
                return;
            }

            _timeAccumulator = 0f;
            _startFrame = _editingService.CursorFrame;
            IsPlaying = true;
        }

        public void Stop()
        {
            _timeAccumulator = 0f;
            _startFrame = 0;
            IsPlaying = false;
        }

        /// <summary>
        /// Playback Update is Forcefully called before all other updates
        /// </summary>
        public void OnUpdate(float delta)
        {
            if (IsPlaying)
            {
                _timeAccumulator += delta;

                var msPerFrame = (1f / _editingService.OpenedSequence.FPS);
                int deltaFrames = (int) (_timeAccumulator / msPerFrame);

                if (deltaFrames != 0)
                {
                    int cursorFrame = _editingService.CursorFrame + deltaFrames;
                    if (cursorFrame >= _editingService.OpenedSequence.FrameLength)
                    {
                        cursorFrame = 0;
                        Stop();
                    }

                    _timeAccumulator -= deltaFrames * msPerFrame;

                    _editingService.CursorFrame = cursorFrame;
                }
            }
        }

        public void OnKeyPressed(Key key)
        {
            if (key == Key.Space)
            {
                if (IsPlaying)
                {
                    Stop();
                }
                else
                {
                    Play();
                }
            }
        }
    }
}