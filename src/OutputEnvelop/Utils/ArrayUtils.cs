using System;

namespace MCIO.OutputEnvelop.Utils
{
    internal static class ArrayUtils
    {
        internal static T[] CopyToExistingArray<T>(T[] targetArray, long targetIndex, ReadOnlyMemory<T> readOnlyMemory)
        {
            Array.Copy(
                sourceArray: readOnlyMemory.Span.ToArray(),
                sourceIndex: 0,
                destinationArray: targetArray,
                destinationIndex: targetIndex,
                length: readOnlyMemory.Length
            );

            return targetArray;
        }
    }
}
