using System;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Editor.Imaging;

namespace SCVE.Editor.Services
{
    public enum PreviewModeType
    {
        None,
        Image,
        Sequence
    }

    public abstract class PreviewMode
    {
        public ThreeWayImage PreviewImage { get; set; }

        public abstract void Sync(float delta);
    }

    public class PreviewModeNone : PreviewMode
    {
        private ThreeWayImage _noPreviewImage;

        public PreviewModeNone(ScveVector2I resolution)
        {
            _noPreviewImage = Utils.CreateNoPreviewImage(resolution.X, resolution.Y);
            _noPreviewImage.ToGpu();
        }

        public override void Sync(float delta)
        {
            PreviewImage = _noPreviewImage;
        }
    }

    public class PreviewModeImage : PreviewMode
    {
        private ThreeWayImage _image;

        public void Set(ThreeWayImage image)
        {
            _image = image;
        }

        public override void Sync(float delta)
        {
            PreviewImage = _image;
        }
    }

    public class PreviewModeSequence : PreviewMode
    {
        private readonly SamplerService _samplerService;
        private readonly EditingService _editingService;
        private ScveVector2I _resolution;

        private Sequence _sequence;

        private ThreeWayImage _sampledFrame;

        private int _frame = -1;
        
        public PreviewModeSequence(SamplerService samplerService, EditingService editingService)
        {
            _samplerService = samplerService;
            _editingService = editingService;
        }

        public void Set(Sequence sequence, ScveVector2I resolution)
        {
            _sequence = sequence;
            _resolution = resolution;
        }

        public override void Sync(float delta)
        {
            if (_frame != _editingService.CursorFrame)
            {
                _sampledFrame?.Dispose();
                _frame = _editingService.CursorFrame;
                _sampledFrame = _samplerService.Sampler.Sample(_sequence, _resolution, _frame);
                _sampledFrame.ToGpu();
                PreviewImage = _sampledFrame;
            }
        }
    }

    public class PreviewService : IService, IUpdateReceiver
    {
        public ThreeWayImage PreviewImage => _currentMode.PreviewImage;

        private static readonly ScveVector2I PreviewResolution = new(1280, 720);

        private PreviewModeNone _modeNone;
        private PreviewModeImage _modeImage;
        private PreviewModeSequence _modeSequence;

        private PreviewMode _currentMode;
        private PreviewModeType _currentPreviewModeType = PreviewModeType.None;

        public PreviewService(EditingService editingService, SamplerService samplerService)
        {
            _modeNone = new PreviewModeNone(PreviewResolution);
            _modeImage = new PreviewModeImage();
            _modeSequence = new PreviewModeSequence(samplerService, editingService);

            SwitchToNone();
        }

        public void SwitchToNone()
        {
            _currentPreviewModeType = PreviewModeType.None;
            _currentMode = _modeNone;
        }

        public void SwitchToImage(ThreeWayImage image)
        {
            _currentPreviewModeType = PreviewModeType.Image;
            _modeImage.Set(image);
            _currentMode = _modeImage;
        }

        public void SwitchToSequence(Sequence sequence)
        {
            _currentPreviewModeType = PreviewModeType.Sequence;
            _modeSequence.Set(sequence, PreviewResolution);
            _currentMode = _modeSequence;
        }

        public void OnUpdate(float delta)
        {
            _currentMode.Sync(delta);
        }
    }
}