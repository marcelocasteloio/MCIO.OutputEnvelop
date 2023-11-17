using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputType;
using MCIO.OutputEnvelop.Models;
using MCIO.OutputEnvelop.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MCIO.OutputEnvelop
{
    public readonly struct OutputEnvelop
    {
        // Properties
        public OutputType Type { get; }
        public OutputMessage[] OutputMessageCollection { get; }
        public Exception[] ExceptionCollection { get; }

        // Constructors
        private OutputEnvelop(
            OutputType type, 
            OutputMessage[] outputMessageCollection,
            Exception[] exceptionCollection
        )
        {
            Type = type;
            OutputMessageCollection = outputMessageCollection ?? Array.Empty<OutputMessage>();
            ExceptionCollection = exceptionCollection ?? Array.Empty<Exception>();
        }

        // Public Methods
        public OutputEnvelop ChangeType(OutputType type) => new OutputEnvelop(type, OutputMessageCollection, ExceptionCollection);

        public OutputEnvelop AddOutputMessage(OutputMessage outputMessage)
        {
            return new OutputEnvelop(
                Type,
                outputMessageCollection: ArrayUtils.AddNewItem(OutputMessageCollection, outputMessage),
                ExceptionCollection
            );
        }
        public OutputEnvelop AddInformationOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateInformation(code, description));
        public OutputEnvelop AddSuccessOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateSuccess(code, description));
        public OutputEnvelop AddWarningOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateWarning(code, description));
        public OutputEnvelop AddErrorOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateError(code, description));
        public OutputEnvelop AddOutputMessageRange(OutputMessage[] outputMessageCollection)
        {
            return new OutputEnvelop(
                Type,
                outputMessageCollection: ArrayUtils.AddRange(OutputMessageCollection, outputMessageCollection),
                ExceptionCollection
            );
        }

        public OutputEnvelop AddException(Exception exception)
        {
            return new OutputEnvelop(
                Type,
                OutputMessageCollection,
                exceptionCollection: ArrayUtils.AddNewItem(ExceptionCollection, exception)
            );
        }
        public OutputEnvelop AddExceptionRange(Exception[] exceptionCollection)
        {
            return new OutputEnvelop(
                Type,
                OutputMessageCollection,
                exceptionCollection: ArrayUtils.AddRange(ExceptionCollection, exceptionCollection)
            );
        }

        public OutputEnvelop ChangeOutputMessageType(string outputMessageCode, OutputMessageType newOutputMessageType)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeType(newOutputMessageType)
                    : outputMessage;
            }

            return Create(Type, OutputMessageCollection, ExceptionCollection);
        }
        public OutputEnvelop ChangeOutputMessageDescription(string outputMessageCode, string newOutputMessageDescription)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeDescription(newOutputMessageDescription)
                    : outputMessage;
            }

            return Create(Type, OutputMessageCollection, ExceptionCollection);
        }
        public OutputEnvelop ChangeOutputMessageTypeAndOutputMessageDescription(string outputMessageCode, OutputMessageType newOutputMessageType, string newOutputMessageDescription)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeTypeAndDescription(newOutputMessageType, newOutputMessageDescription)
                    : outputMessage;
            }

            return Create(Type, OutputMessageCollection, ExceptionCollection);
        }

        public static OutputEnvelop Execute(Func<OutputEnvelop> handler)
        {
            try
            {
                return handler();
            }
            catch (Exception ex)
            {
                return Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }
        public static OutputEnvelop Execute<TInput>(TInput input, Func<TInput, OutputEnvelop> handler)
        {
            try
            {
                return handler(input);
            }
            catch (Exception ex)
            {
                return Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }
        public static Task<OutputEnvelop> ExecuteAsync(Func<CancellationToken, Task<OutputEnvelop>> handler, CancellationToken cancellationToken)
        {
            try
            {
                return handler(cancellationToken);
            }
            catch (Exception ex)
            {
                return Task.FromResult(Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex }));
            }
        }
        public static Task<OutputEnvelop> ExecuteAsync<TInput>(TInput input, Func<TInput, CancellationToken, Task<OutputEnvelop>> handler, CancellationToken cancellationToken)
        {
            try
            {
                return handler(input, cancellationToken);
            }
            catch (Exception ex)
            {
                return Task.FromResult(Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex }));
            }
        }

        // Builders
        public static OutputEnvelop Create(OutputType type) => new OutputEnvelop(type, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop Create(OutputType type, OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
        {
            // Validate
            InvalidOutputTypeException.ThrowIfInvalid(type);

            // Process and return
            return new OutputEnvelop(
                type, 
                outputMessageCollection,
                exceptionCollection
            );
        }
        public static OutputEnvelop Create(OutputType type, OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop Create(OutputType type, params OutputEnvelop[] outputEnvelopCollection)
        {
            // Create new output message collection and exception message collection
            var newMessageOutputCollectionLength = 0L;
            var newExceptionCollectionLength = 0L;

            for (int i = 0; i < outputEnvelopCollection.Length; i++)
            {
                newMessageOutputCollectionLength += outputEnvelopCollection[i].OutputMessageCollection.Length;
                newExceptionCollectionLength += outputEnvelopCollection[i].ExceptionCollection.Length;
            }

            var newMessageOutputCollection = new OutputMessage[newMessageOutputCollectionLength];
            var newExceptionCollection = new Exception[newExceptionCollectionLength];

            // copy all output message collection and exception message collection to new arrays
            var lastNewOutputMessageCollectionOffset = 0L;
            var lastExceptionMessageCollectionOffset = 0L;

            for (int i = 0; i < outputEnvelopCollection.Length; i++)
            {
                var outputEnvelop = outputEnvelopCollection[i];

                // Copy MessageOutputCollection
                ArrayUtils.CopyToExistingArray(
                    targetArray: newMessageOutputCollection,
                    targetIndex: lastNewOutputMessageCollectionOffset,
                    sourceArray: outputEnvelop.OutputMessageCollection
                );
                lastNewOutputMessageCollectionOffset += outputEnvelop.OutputMessageCollection.Length;

                // Copy ExceptionCollection
                ArrayUtils.CopyToExistingArray(
                    targetArray: newExceptionCollection,
                    targetIndex: lastExceptionMessageCollectionOffset,
                    sourceArray: outputEnvelop.ExceptionCollection
                );
                lastExceptionMessageCollectionOffset += outputEnvelop.ExceptionCollection.Length;
            }

            return Create(
                type,
                outputMessageCollection: newMessageOutputCollection,
                exceptionCollection: newExceptionCollection
            );
        }
        public static OutputEnvelop Create(params OutputEnvelop[] outputEnvelopCollection)
        {
            // Analyze Type
            var hasInformationMessageType = false;
            var hasSuccessMessageType = false;
            var hasWarningMessageType = false;
            var hasErrorMessageType = false;
            var hasException = false;

            // Analyze all output envelops
            for (int outputEnvelopIndex = 0; outputEnvelopIndex < outputEnvelopCollection.Length; outputEnvelopIndex++)
            {
                var outputEnvelop = outputEnvelopCollection[outputEnvelopIndex];

                // Analyze output messages
                for (int outputMessageIndex = 0; outputMessageIndex < outputEnvelop.OutputMessageCollection.Length; outputMessageIndex++)
                {
                    var outputMessage = outputEnvelop.OutputMessageCollection[outputMessageIndex];

                    if(!hasInformationMessageType && outputMessage.Type == OutputMessageType.Information)
                        hasInformationMessageType = true;

                    if (!hasSuccessMessageType && outputMessage.Type == OutputMessageType.Success)
                        hasSuccessMessageType = true;

                    if (!hasWarningMessageType && outputMessage.Type == OutputMessageType.Warning)
                        hasWarningMessageType = true;

                    if (!hasErrorMessageType && outputMessage.Type == OutputMessageType.Error)
                        hasErrorMessageType = true;
                }

                // Analyze exceptions
                if (hasException)
                    continue;

                hasException = outputEnvelop.ExceptionCollection.Length > 0;
            }

            var type =
                hasSuccessMessageType
                ? hasErrorMessageType || hasException ? OutputType.Partial : OutputType.Success
                : hasErrorMessageType || hasException ? OutputType.Error : OutputType.Success;

            return Create(type, outputEnvelopCollection);
        }

        public static OutputEnvelop CreateSuccess() => new OutputEnvelop(type: OutputType.Success, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop CreateSuccess(OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
        {
            return Create(type: OutputType.Success, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreateSuccess(OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type: OutputType.Success, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop CreateSuccess(params OutputEnvelop[] outputEnvelopCollection)
        {
            return Create(type: OutputType.Success, outputEnvelopCollection);
        }

        public static OutputEnvelop CreatePartial() => new OutputEnvelop(type: OutputType.Partial, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop CreatePartial(OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
        {
            return Create(type: OutputType.Partial, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreatePartial(OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type: OutputType.Partial, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop CreatePartial(params OutputEnvelop[] outputEnvelopCollection)
        {
            return Create(type: OutputType.Partial, outputEnvelopCollection);
        }

        public static OutputEnvelop CreateError() => new OutputEnvelop(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop CreateError(OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
        {
            return Create(type: OutputType.Error, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreateError(OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type: OutputType.Error, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop CreateError(params OutputEnvelop[] outputEnvelopCollection)
        {
            return Create(type: OutputType.Error, outputEnvelopCollection);
        }
    }
}
