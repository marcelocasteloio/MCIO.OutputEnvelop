using System;

namespace MCIO.OutputEnvelop.Utils
{
    internal static class ReadOnlyMemoryUtils
    {
        internal static ReadOnlyMemory<T> AddNewItem<T>(ReadOnlyMemory<T> sourceReadOnlyMemory, T item)
        {
            // Create new ReadOnlyMemory
            var newReadOnlyMemory = new T[sourceReadOnlyMemory.Length + 1];

            // Copy data
            sourceReadOnlyMemory.Span.CopyTo(newReadOnlyMemory);

            // Add new item in last array position
            newReadOnlyMemory[newReadOnlyMemory.Length - 1] = item;

            return newReadOnlyMemory;
        }
        internal static ReadOnlyMemory<T> AddRange<T>(ReadOnlyMemory<T> sourceReadOnlyMemory, ReadOnlyMemory<T> readOnlyMemory)
        {
            // Create new ReadOnlyMemory
            var newReadOnlyMemory = new T[sourceReadOnlyMemory.Length + readOnlyMemory.Length];

            // Copy data
            sourceReadOnlyMemory.Span.CopyTo(newReadOnlyMemory);

            // Add new item in last array position
            readOnlyMemory.CopyTo(newReadOnlyMemory);

            return newReadOnlyMemory;
        }
    }
}
