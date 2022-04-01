using System;
using System.IO;
using System.Threading.Tasks;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Editing.Editing;
using SCVE.Editor.Editing.Misc;
using SCVE.Engine.ImageSharpBindings;
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

        // TODO: pass resolution
        public void Export(Sequence sequence, string exportDirectoryLocation)
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
                var frame = _samplerService.Sampler.Sample(sequence, new ScveVector2I(1280, 720), i);
                var framePath = Path.Combine(exportDirectoryPath, i + ".png");
                
                textureWriter.Save(frame.ToByteArray(), frame.Width, frame.Height, framePath);

                Progress = (float)i / sequence.FrameLength;

                Task.Delay(200);
            }
        }
    }
}