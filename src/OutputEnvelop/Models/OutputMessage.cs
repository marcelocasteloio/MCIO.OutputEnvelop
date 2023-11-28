﻿using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputMessageType;
using System;

namespace MCIO.OutputEnvelop.Models
{
    public readonly struct OutputMessage
    {
        // Properties
        public OutputMessageType Type { get; }
        public string Code { get; }
        public string Description { get; }

        // Constructors
        private OutputMessage(OutputMessageType type, string code, string description)
        {
            Type = type;
            Code = code;
            Description = description;
        }

        // Public Methods
        public OutputMessage ChangeType(OutputMessageType type) => Create(type, Code, Description);
        public OutputMessage ChangeDescription(string description) => Create(Type, Code, description);
        public OutputMessage ChangeTypeAndDescription(OutputMessageType type, string description) => Create(type, Code, description);

        // Builders
        public static OutputMessage Create(OutputMessageType type, string code, string description = null)
        {
            // Validate
            InvalidOutputMessageTypeException.ThrowIfInvalid(type);

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            // Process and return
            return new OutputMessage(type, code, description);
        }
        public static OutputMessage CreateInformation(string code, string description = null) => Create(OutputMessageType.Information, code, description);
        public static OutputMessage CreateSuccess(string code, string description = null) => Create(OutputMessageType.Success, code, description);
        public static OutputMessage CreateWarning(string code, string description = null) => Create(OutputMessageType.Warning, code, description);
        public static OutputMessage CreateError(string code, string description = null) => Create(OutputMessageType.Error, code, description);
    }
}
