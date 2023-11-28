using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputEnvelopType;
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
        public OutputEnvelopType Type { get; }
        public ReadOnlyMemory<OutputMessage> OutputMessageCollection { get; }
        public ReadOnlyMemory<Exception> ExceptionCollection { get; }

        // Constructors
        private OutputEnvelop(
            OutputEnvelopType type,
            ReadOnlyMemory<OutputMessage> outputMessageCollection,
            ReadOnlyMemory<Exception> exceptionCollection
        )
        {
            Type = type;
            OutputMessageCollection = outputMessageCollection;
            ExceptionCollection = exceptionCollection;
        }

        // Public Methods
        public OutputEnvelop ChangeType(OutputEnvelopType type) => new OutputEnvelop(type, OutputMessageCollection, ExceptionCollection);

        public OutputEnvelop AddOutputMessage(OutputMessage outputMessage)
        {
            return new OutputEnvelop(
                Type,
                outputMessageCollection: ReadOnlyMemoryUtils.AddNewItem(OutputMessageCollection, outputMessage),
                ExceptionCollection
            );
        }
        public OutputEnvelop AddOutputMessageCollection(ReadOnlyMemory<OutputMessage> outputMessageCollection)
        {
            return new OutputEnvelop(
                Type,
                outputMessageCollection: ReadOnlyMemoryUtils.AddRange(OutputMessageCollection, outputMessageCollection),
                ExceptionCollection
            );
        }
        public OutputEnvelop AddInformationOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateInformation(code, description));
        public OutputEnvelop AddSuccessOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateSuccess(code, description));
        public OutputEnvelop AddWarningOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateWarning(code, description));
        public OutputEnvelop AddErrorOutputMessage(string code, string description) => AddOutputMessage(OutputMessage.CreateError(code, description));

        public OutputEnvelop AddException(Exception exception)
        {
            return new OutputEnvelop(
                Type,
                OutputMessageCollection,
                exceptionCollection: ReadOnlyMemoryUtils.AddNewItem(ExceptionCollection, exception)
            );
        }
        public OutputEnvelop AddExceptionCollection(ReadOnlyMemory<Exception> exceptionCollection)
        {
            return new OutputEnvelop(
                Type,
                OutputMessageCollection,
                exceptionCollection: ReadOnlyMemoryUtils.AddRange(ExceptionCollection, exceptionCollection)
            );
        }

        public OutputEnvelop ChangeOutputMessageType(string outputMessageCode, OutputMessageType newOutputMessageType)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection.Span[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeType(newOutputMessageType)
                    : outputMessage;
            }

            return Create(
                Type, 
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(newOutputMessageCollection), 
                ExceptionCollection
            );
        }
        public OutputEnvelop ChangeOutputMessageDescription(string outputMessageCode, string newOutputMessageDescription)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection.Span[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeDescription(newOutputMessageDescription)
                    : outputMessage;
            }

            return Create(Type, newOutputMessageCollection, ExceptionCollection);
        }
        public OutputEnvelop ChangeOutputMessageTypeAndOutputMessageDescription(string outputMessageCode, OutputMessageType newOutputMessageType, string newOutputMessageDescription)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection.Span[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeTypeAndDescription(newOutputMessageType, newOutputMessageDescription)
                    : outputMessage;
            }

            return Create(Type, newOutputMessageCollection, ExceptionCollection);
        }

        public static OutputEnvelop Execute(Func<OutputEnvelop> handler)
        {
            try
            {
                return handler();
            }
            catch (Exception ex)
            {
                return Create(type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
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
                return Create(type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }
        public static async Task<OutputEnvelop> ExecuteAsync(Func<CancellationToken, Task<OutputEnvelop>> handler, CancellationToken cancellationToken)
        {
            try
            {
                /*
                 * to can capture trowed exception inside handler
                 * we need await the handler execution
                 */
                return await handler(cancellationToken);
            }
            catch (Exception ex)
            {
                return Create(type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }
        public static async Task<OutputEnvelop> ExecuteAsync<TInput>(TInput input, Func<TInput, CancellationToken, Task<OutputEnvelop>> handler, CancellationToken cancellationToken)
        {
            try
            {
                /*
                 * to can capture trowed exception inside handler
                 * we need await the handler execution
                 */
                return await handler(input, cancellationToken);
            }
            catch (Exception ex)
            {
                return Create(type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }

        // Builders
        public static OutputEnvelop Create(OutputEnvelopType type) => Create(type, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop Create(OutputEnvelopType type, ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            // Validate
            InvalidOutputEnvelopTypeException.ThrowIfInvalid(type);

            // Process and return
            return new OutputEnvelop(
                type, 
                outputMessageCollection,
                exceptionCollection
            );
        }
        public static OutputEnvelop Create(OutputEnvelopType type, OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop Create(OutputEnvelopType type, params OutputEnvelop[] outputEnvelopCollection)
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
                    readOnlyMemory: outputEnvelop.OutputMessageCollection
                );
                lastNewOutputMessageCollectionOffset += outputEnvelop.OutputMessageCollection.Length;

                // Copy ExceptionCollection
                ArrayUtils.CopyToExistingArray(
                    targetArray: newExceptionCollection,
                    targetIndex: lastExceptionMessageCollectionOffset,
                    readOnlyMemory: outputEnvelop.ExceptionCollection
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
            var hasSuccessType = false;
            var hasPartialType = false;
            var hasErrorType = false;

            // Analyze all output envelops
            for (int outputEnvelopIndex = 0; outputEnvelopIndex < outputEnvelopCollection.Length; outputEnvelopIndex++)
            {
                var outputEnvelop = outputEnvelopCollection[outputEnvelopIndex];

                if (!hasSuccessType && outputEnvelop.Type == OutputEnvelopType.Success)
                    hasSuccessType = true;
                else if (!hasPartialType && outputEnvelop.Type == OutputEnvelopType.Partial)
                    hasPartialType = true;
                else if (!hasErrorType && outputEnvelop.Type == OutputEnvelopType.Error)
                    hasErrorType = true;
            }

            var type = default(OutputEnvelopType);

            if (hasPartialType)
                type = OutputEnvelopType.Partial;
            else if(hasSuccessType)
                type = hasErrorType ? OutputEnvelopType.Partial : OutputEnvelopType.Success;
            else if(hasErrorType)
                type = OutputEnvelopType.Error;

            return Create(type, outputEnvelopCollection);
        }
        public static OutputEnvelop Create(OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
        {
            // Analyze Type
            var hasSuccessMessageType = false;
            var hasErrorMessageType = false;

            // Analyze output messages
            for (int outputMessageIndex = 0; outputMessageIndex < outputMessageCollection.Length; outputMessageIndex++)
            {
                var outputMessage = outputMessageCollection[outputMessageIndex];

                if (!hasSuccessMessageType && outputMessage.Type == OutputMessageType.Success)
                    hasSuccessMessageType = true;

                if (!hasErrorMessageType && outputMessage.Type == OutputMessageType.Error)
                    hasErrorMessageType = true;
            }

            var hasException = exceptionCollection.Length > 0;

            var type =
                hasSuccessMessageType
                ? hasErrorMessageType || hasException ? OutputEnvelopType.Partial : OutputEnvelopType.Success
                : hasErrorMessageType || hasException ? OutputEnvelopType.Error : OutputEnvelopType.Success;

            return Create(type, outputMessageCollection, exceptionCollection);
        }

        public static OutputEnvelop CreateSuccess() => new OutputEnvelop(type: OutputEnvelopType.Success, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop CreateSuccess(ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return Create(type: OutputEnvelopType.Success, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreateSuccess(OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type: OutputEnvelopType.Success, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop CreateSuccess(params OutputEnvelop[] outputEnvelopCollection)
        {
            return Create(type: OutputEnvelopType.Success, outputEnvelopCollection);
        }

        public static OutputEnvelop CreatePartial() => new OutputEnvelop(type: OutputEnvelopType.Partial, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop CreatePartial(ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return Create(type: OutputEnvelopType.Partial, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreatePartial(OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type: OutputEnvelopType.Partial, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop CreatePartial(params OutputEnvelop[] outputEnvelopCollection)
        {
            return Create(type: OutputEnvelopType.Partial, outputEnvelopCollection);
        }

        public static OutputEnvelop CreateError() => new OutputEnvelop(type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: null);
        public static OutputEnvelop CreateError(ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return Create(type: OutputEnvelopType.Error, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreateError(OutputEnvelop outputEnvelop)
        {
            return new OutputEnvelop(type: OutputEnvelopType.Error, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop CreateError(params OutputEnvelop[] outputEnvelopCollection)
        {
            return Create(type: OutputEnvelopType.Error, outputEnvelopCollection);
        }
    }
}
