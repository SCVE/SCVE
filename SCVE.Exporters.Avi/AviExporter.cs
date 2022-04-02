// Contains common types for AVI format like FourCC

using SCVE.Editor.Editing.Misc;
using SharpAvi;
// Contains types used for writing like AviWriter
using SharpAvi.Output;
// Contains types related to encoding like Mpeg4VcmVideoEncoder
using SharpAvi.Codecs;

namespace SCVE.Exporters.Avi;

public class AviExporter : IDisposable
{
    private AviWriter _writer;
    private IAviVideoStream _videoStream;

    public AviExporter(string filePath, ScveVector2I resolution, int fps)
    {
        _writer = new AviWriter(filePath)
        {
            FramesPerSecond = fps,
            // Emitting the AVI v1 index in addition to the OpenDML index (AVI v2)
            // improves compatibility with some software, including 
            // standard Windows programs like Media Player and File Explorer
            EmitIndex1 = true
        };

        // returns IAviVideoStream
        _videoStream = _writer.AddVideoStream();

        // set standard VGA resolution
        _videoStream.Width = resolution.X;
        _videoStream.Height = resolution.Y;

        // class SharpAvi.CodecIds contains FOURCCs for several well-known codecs
        // Uncompressed is the default value, just set it for clarity

        _videoStream.Codec = CodecIds.Uncompressed;

        // Uncompressed format requires to also specify bits per pixel
        _videoStream.BitsPerPixel = BitsPerPixel.Bpp32;
        
    }

    public void EndStream()
    {
        _writer.Close();
    }
    
    public void WriteFrame(byte[] pixels)
    {
        // NOTE: BGR32 input 
        
        // write data to a frame
        _videoStream.WriteFrame(true, pixels);
    }
    
    public async Task WriteFrameAsync(byte[] pixels)
    {
        // write data to a frame
        await _videoStream.WriteFrameAsync(true, pixels);
    }

    public void Dispose()
    {
        ((IDisposable) _writer).Dispose();
    }
}