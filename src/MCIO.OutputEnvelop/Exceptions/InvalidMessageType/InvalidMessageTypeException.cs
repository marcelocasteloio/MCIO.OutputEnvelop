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

        // Builders
        public static void ThrowIfInvalid(OutputMessageType outputMessageType)
        {
            if (!Enum.IsDefined(typeof(OutputMessageType), outputMessageType))
                throw new InvalidMessageTypeException(outputMessageType);
        }
    }
}
