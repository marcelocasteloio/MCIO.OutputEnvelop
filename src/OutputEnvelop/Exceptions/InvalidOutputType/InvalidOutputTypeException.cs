using MCIO.OutputEnvelop.Enums;
using System;

namespace MCIO.OutputEnvelop.Exceptions.InvalidOutputType
{
    public class InvalidOutputTypeException
        : Exception
    {
        // Properties
        public OutputType OutputType { get; }

        // Constructors
        private InvalidOutputTypeException(OutputType outputType)
        {
            OutputType = outputType;
        }

        // Builders
        public static void ThrowIfInvalid(OutputType outputType)
        {
            /*
             * The bool IsDefined(Type enumType, Object value) alloc in heap
             * https://github.com/microsoft/referencesource/blob/master/mscorlib/system/enum.cs#L596
             * 
             * To avoid these allocation, I decide couple the enum value range in this method
             * instead of heap allocation.
             * 
             * Alternative code that alloc:
             * 
             * if (!Enum.IsDefined(typeof(OutputType), outputType))
             *      throw new InvalidOutputTypeException(outputType);
             */
            var value = (int)outputType;
            if (value < 0 || value > 3)
                throw new InvalidOutputTypeException(outputType);
        }
    }
}
