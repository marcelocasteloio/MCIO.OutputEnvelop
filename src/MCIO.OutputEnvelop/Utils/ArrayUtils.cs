using System;

namespace MCIO.OutputEnvelop.Utils
{
    internal static class ArrayUtils
    {
        internal static T[] AddNewItem<T>(T[] sourceArray, T item)
        {
            // Create new array
            var newArray = new T[sourceArray.Length + 1];

            // Copy data between sourceArray and new array
            for (int i = 0; i < sourceArray.Length; i++)
                newArray[i] = sourceArray[i];

            // Add new item in last array position
            newArray[newArray.Length - 1] = item;

            return newArray;
        }
        internal static T[] AddRange<T>(T[] sourceArray, T[] itemArray)
        {
            // Create new array
            var newArray = new T[sourceArray.Length + itemArray.Length];

            // Copy data between sourceArray and new array
            for (int i = 0; i < sourceArray.Length; i++)
                newArray[i] = sourceArray[i];

            // Copy itemArray to new array
            CopyToExistingArray(
                targetArray: newArray,
                targetIndex: sourceArray.Length,
                sourceArray: itemArray
            );

            return newArray;
        }
        internal static T[] CopyToExistingArray<T>(T[] targetArray, long targetIndex, T[] sourceArray)
        {
            Array.Copy(
                sourceArray,
                sourceIndex: 0,
                destinationArray: targetArray,
                destinationIndex: targetIndex,
                length: sourceArray.Length
            );

            return targetArray;
        }
    }
}
