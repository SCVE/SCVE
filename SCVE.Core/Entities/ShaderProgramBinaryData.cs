namespace SCVE.Core.Entities
{
    public class ShaderProgramBinaryData
    {
        public byte[] Data { get; set; }

        public int Extension { get; set; }

        public ShaderProgramBinaryData(byte[] data, int extension)
        {
            Data = data;
            Extension = extension;
        }
    }
}