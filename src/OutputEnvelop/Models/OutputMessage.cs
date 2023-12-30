using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputMessageType;
using System;

namespace MCIO.OutputEnvelop.Models
{
    public readonly struct OutputMessage<T>
    {
        // Properties
        public OutputMessageType Type { get; }
        public T Code { get; }
        public string Description { get; }

        // Constructors
        private OutputMessage(OutputMessageType type, T code, string description)
        {
            Type = type;
            Code = code;
            Description = description;
        }

        // Public Methods
        public OutputMessage<T> ChangeType(OutputMessageType type) => Create(type, Code, Description);
        public OutputMessage<T> ChangeDescription(string description) => Create(Type, Code, description);
        public OutputMessage<T> ChangeTypeAndDescription(OutputMessageType type, string description) => Create(type, Code, description);

        // Builders
        public static OutputMessage<T> Create(OutputMessageType type, T code, string description = null)
        {
            // Validate
            InvalidOutputMessageTypeException.ThrowIfInvalid(type);

            // Process and return
            return new OutputMessage<T>(type, code, description);
        }
        public static OutputMessage<T> CreateInformation(T code, string description = null) => Create(OutputMessageType.Information, code, description);
        public static OutputMessage<T> CreateSuccess(T code, string description = null) => Create(OutputMessageType.Success, code, description);
        public static OutputMessage<T> CreateWarning(T code, string description = null) => Create(OutputMessageType.Warning, code, description);
        public static OutputMessage<T> CreateError(T code, string description = null) => Create(OutputMessageType.Error, code, description);
    }
    public readonly struct OutputMessage
    {
        // Fields
        private readonly OutputMessage<string> _outputMessage;

        // Properties
        public OutputMessageType Type => _outputMessage.Type;
        public string Code => _outputMessage.Code;
        public string Description => _outputMessage.Description;

        // Constructors
        public OutputMessage(OutputMessage<string> outputMessage)
        {
            _outputMessage = outputMessage;
        }

        // Public Methods
        public OutputMessage ChangeType(OutputMessageType type) => new OutputMessage(_outputMessage.ChangeType(type));
        public OutputMessage ChangeDescription(string description) => new OutputMessage(_outputMessage.ChangeDescription(description));
        public OutputMessage ChangeTypeAndDescription(OutputMessageType type, string description) => new OutputMessage(_outputMessage.ChangeTypeAndDescription(type, description));

        // Builders
        public static OutputMessage Create(OutputMessageType type, string code, string description = null) => new OutputMessage(OutputMessage<string>.Create(type, code, description));
        public static OutputMessage CreateInformation(string code, string description = null) => new OutputMessage(OutputMessage<string>.CreateInformation(code, description));
        public static OutputMessage CreateSuccess(string code, string description = null) => new OutputMessage(OutputMessage<string>.CreateSuccess(code, description));
        public static OutputMessage CreateWarning(string code, string description = null) => new OutputMessage(OutputMessage<string>.CreateWarning(code, description));
        public static OutputMessage CreateError(string code, string description = null) => new OutputMessage(OutputMessage<string>.CreateError(code, description));
    }
}
