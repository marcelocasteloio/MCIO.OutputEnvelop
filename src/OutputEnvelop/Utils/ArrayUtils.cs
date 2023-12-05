using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MCIO.OutputEnvelop.UnitTests")]
namespace MCIO.OutputEnvelop.Utils
{
    internal static class ArrayUtils
    {
        internal static T[] CopyToExistingArray<T>(T[] destinationArray, long destinationIndex, T[] sourceArray)
        {
            Array.Copy(
                sourceArray: sourceArray,
                sourceIndex: 0,
                destinationArray,
                destinationIndex,
                length: sourceArray.Length
            );

            return destinationArray;
        }
        internal static T[] CopyToExistingArray<T>(T[] destinationArray, long destinationIndex, IEnumerable<T> sourceArray)
        {
            var counter = destinationIndex;

            foreach (var item in sourceArray)
                destinationArray[counter++] = item;

            return destinationArray;
        }
        internal static T[] AddNewItem<T>(T[] sourceArray, T newItem)
        {
            if (sourceArray == null)
                return newItem == null ? null : (new T[] { newItem });

            if (newItem == null)
                return sourceArray;

            var destinationArray = new T[sourceArray.Length + 1];

            CopyToExistingArray(destinationArray, destinationIndex: 0, sourceArray);

            destinationArray[destinationArray.Length - 1] = newItem;

            return destinationArray;
        }
        internal static T[] AddRange<T>(T[] sourceArray, T[] items)
        {
            if (sourceArray == null)
                return items ?? null;

            if (items == null)
                return sourceArray;

            var destinationArray = new T[sourceArray.Length + items.Length];

            CopyToExistingArray(destinationArray, destinationIndex: 0, sourceArray);
            CopyToExistingArray(destinationArray, destinationIndex: sourceArray.Length, items);

            return destinationArray;
        }
    }
}
