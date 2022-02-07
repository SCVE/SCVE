namespace SCVE.Editor.MemoryUtils;

public class ByteArrayPoolItem
{
    public byte[] Bytes { get; }
    public bool IsFree { get; set; }

    public ByteArrayPoolItem(int length)
    {
        Bytes = new byte[length];
    }
}