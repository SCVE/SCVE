using System;
using System.Collections.Generic;

namespace SCVE.Editor.MemoryUtils;

public class ByteArrayPool
{
    private List<ByteArrayPoolItem> _arrays;

    /// <summary>
    /// Maximum amount of arrays
    /// </summary>
    public int Capacity { get; init; }

    /// <summary>
    /// Length of a single array
    /// </summary>
    public int Length { get; init; }

    public ByteArrayPool(int length, int capacity)
    {
        Capacity = capacity;
        Length = length;
        _arrays = new List<ByteArrayPoolItem>(capacity);
    }

    public ByteArrayPoolItem GetFree()
    {
        // if there are less allocated byte arrays, than the capacity - allocate new and don't worry
        if (_arrays.Count < Capacity)
        {
            var byteArrayPoolItem = new ByteArrayPoolItem(Length)
            {
                IsFree = false
            };
            _arrays.Add(byteArrayPoolItem);
            return byteArrayPoolItem;
        }

        // in case of no free slots - find an empty one
        for (var i = 0; i < _arrays.Count; i++)
        {
            if (_arrays[i].IsFree)
            {
                _arrays[i].IsFree = false;
                return _arrays[i];
            }
        }

        throw new ArgumentOutOfRangeException("No free arrays available");
    }

    public void Return(ByteArrayPoolItem item)
    {
        var index = _arrays.FindIndex(i => i == item);
        if (index == -1)
        {
            throw new ArgumentOutOfRangeException("Returned pool item doesn't belong to this pool");
        }

        _arrays[index].IsFree = true;
    }
}