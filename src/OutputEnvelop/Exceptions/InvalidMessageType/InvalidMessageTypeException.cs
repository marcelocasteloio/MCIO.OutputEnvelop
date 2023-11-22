using MCIO.OutputEnvelop.Enums;
using System;

namespace MCIO.OutputEnvelop.Exceptions.InvalidMessageType
{
    public class InvalidMessageTypeException
        : Exception
    {
        // Properties
        public OutputMessageType MessageType { get; }

        // Constructors
        private InvalidMessageTypeException(OutputMessageType outputMessageType)
        {
            MessageType = outputMessageType;
        }

        // Public Methods
        public static void ThrowIfInvalid(OutputMessageType outputMessageType)
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
             * if (!Enum.IsDefined(typeof(OutputMessageType), outputMessageType))
             *      throw new InvalidMessageTypeException(outputMessageType);
             */
            var value = (int)outputMessageType;
            if (value < 1 || value > 4)
                throw new InvalidMessageTypeException(outputMessageType);
        }
    }
}
