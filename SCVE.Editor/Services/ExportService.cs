using System;
using System.IO;
using System.Threading.Tasks;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Engine.ImageSharpBindings;
using SCVE.Exporters.Avi;
using Silk.NET.SDL;

namespace SCVE.Editor.Services
{
    public class ExportService : IService
    {
        private SamplerService _samplerService;

        public float Progress { get; set; }

        public ExportService(SamplerService samplerService)
        {
            _samplerService = samplerService;
        }

        public void ExportPngSequence(Sequence sequence, ScveVector2I resolution, string exportDirectoryLocation)
        {
            var exportDirectoryPath = Path.Combine(exportDirectoryLocation, sequence.Title);
            var exportDirectoryInfo = new DirectoryInfo(exportDirectoryPath);
            if (exportDirectoryInfo.Exists)
            {
                exportDirectoryInfo.ClearContent();

                Console.WriteLine($"Cleared directory {sequence.Title}");
            }
            else
            {
                exportDirectoryInfo.Create();

                Console.WriteLine($"Created directory {sequence.Title}");
            }

            var textureWriter = new ImageSharpTextureWriter();
            for (var i = 0; i < sequence.FrameLength; i++)
            {
                var frame = _samplerService.Sampler.Sample(sequence, resolution, i);
                var framePath = Path.Combine(exportDirectoryPath, i + ".png");

                textureWriter.Save(frame.ToByteArray(), frame.Width, frame.Height, framePath);

                Progress = (float) i / sequence.FrameLength;

                Task.Delay(200);
            }
        }

        public void ExportAvi(Sequence sequence, ScveVector2I resolution, string exportDirectoryLocation)
        {
            var exportFilePath = Path.Combine(exportDirectoryLocation, sequence.Title + ".avi");

            using var aviExporter = new AviExporter(exportFilePath, resolution, sequence.FPS);
            for (var i = 0; i < sequence.FrameLength; i++)
            {
                var frame = _samplerService.Sampler.Sample(sequence, resolution, i);

                var rgbaPixels = frame.ToByteArray();

                ImageSharpImageFlipper.FlipY(rgbaPixels, frame.Width, frame.Height);

                Utils.ShuffleRgba32ToBgr32(rgbaPixels);

                aviExporter.WriteFrame(rgbaPixels);

                Progress = (float) i / sequence.FrameLength;
            }
        }
    }
}