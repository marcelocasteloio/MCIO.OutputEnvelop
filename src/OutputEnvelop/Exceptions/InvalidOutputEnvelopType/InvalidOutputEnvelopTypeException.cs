using MCIO.OutputEnvelop.Enums;
using System;

namespace MCIO.OutputEnvelop.Exceptions.InvalidOutputEnvelopType
{
    public class InvalidOutputEnvelopTypeException
        : Exception
    {
        // Properties
        public OutputEnvelopType OutputEnvelopType { get; }

        // Constructors
        private InvalidOutputEnvelopTypeException(OutputEnvelopType outputEnvelopType)
        {
            OutputEnvelopType = outputEnvelopType;
        }

        // Builders
        public static void ThrowIfInvalid(OutputEnvelopType outputEnvelopType)
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
             * if (!Enum.IsDefined(typeof(OutputType), outputEnvelopType))
             *      throw new InvalidOutputTypeException(outputEnvelopType);
             */
            var value = (int)outputEnvelopType;
            if (value < 1 || value > 3)
                throw new InvalidOutputEnvelopTypeException(outputEnvelopType);
        }
    }
}
