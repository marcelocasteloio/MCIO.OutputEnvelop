using MCIO.OutputEnvelop.Enums;

namespace MCIO.OutputEnvelop.Exceptions.InvalidMessageType;

public class InvalidMessageTypeException
    : Exception
{
    // Properties
    public MessageType MessageType { get; }

    // Constructors
    private InvalidMessageTypeException(MessageType messageType)
    {
        MessageType = messageType;
    }

    // Builders
    public static void ThrowIfInvalid(MessageType messageType)
    {
        if (!Enum.IsDefined(messageType))
            throw new InvalidMessageTypeException(messageType);
    }
}
