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
            if (!Enum.IsDefined(typeof(OutputType), outputType))
                throw new InvalidOutputTypeException(outputType);
        }
    }
}
