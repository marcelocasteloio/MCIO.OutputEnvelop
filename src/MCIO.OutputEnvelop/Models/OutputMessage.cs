using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidMessageType;

namespace MCIO.OutputEnvelop.Models;

public readonly record struct OutputMessage
{
    // Properties
    public MessageType Type { get; }
    public string Code { get; }
    public string? Description { get; }

    // Constructors
    private OutputMessage(MessageType type, string code, string? description)
    {
        Type = type;
        Code = code;
        Description = description;
    }

    // Public Methods
    public OutputMessage ChangeType(MessageType type) => Create(type, Code, Description);
    public OutputMessage ChangeDescription(string? description) => Create(Type, Code, description);
    public OutputMessage ChangeTypeAndDescription(MessageType type, string? description) => Create(type, Code, description);

    // Builders
    public static OutputMessage Create(MessageType type, string code, string? description = null)
    {
        // Validate
        InvalidMessageTypeException.ThrowIfInvalid(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        // Process and return
        return new OutputMessage(type, code, description);
    }
}
