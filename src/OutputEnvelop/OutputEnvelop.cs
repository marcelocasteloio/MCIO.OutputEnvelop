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
        // Fields
        private readonly OutputEnvelop<object> _outputEnvelop;

        // Properties
        public OutputEnvelopType Type => _outputEnvelop.Type;
        public ReadOnlyMemory<OutputMessage> OutputMessageCollection => _outputEnvelop.OutputMessageCollection;
        public ReadOnlyMemory<Exception> ExceptionCollection => _outputEnvelop.ExceptionCollection;

        // Constructors
        private OutputEnvelop(
            OutputEnvelopType type,
            ReadOnlyMemory<OutputMessage> outputMessageCollection,
            ReadOnlyMemory<Exception> exceptionCollection
        )
        {
            _outputEnvelop = OutputEnvelop<object>.Create(
                output: null,
                type,
                outputMessageCollection,
                exceptionCollection
            );
        }

        // Public Methods
        public OutputEnvelop ChangeType(OutputEnvelopType type)
        {
            return _outputEnvelop.ChangeType(type);
        }

        public OutputEnvelop AddOutputMessage(OutputMessage outputMessage)
        {
            return _outputEnvelop.AddOutputMessage(outputMessage);
        }
        public OutputEnvelop AddOutputMessageCollection(ReadOnlyMemory<OutputMessage> outputMessageCollection)
        {
            return _outputEnvelop.AddOutputMessageCollection(outputMessageCollection);
        }

        public OutputEnvelop AddInformationOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateInformation(code, description));
        }
        public OutputEnvelop AddSuccessOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateSuccess(code, description));
        }
        public OutputEnvelop AddWarningOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateWarning(code, description));
        }
        public OutputEnvelop AddErrorOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateError(code, description));
        }

        public OutputEnvelop AddException(Exception exception)
        {
            return _outputEnvelop.AddException(exception);
        }
        public OutputEnvelop AddExceptionCollection(ReadOnlyMemory<Exception> exceptionCollection)
        {
            return _outputEnvelop.AddExceptionCollection(exceptionCollection);
        }

        public OutputEnvelop ChangeOutputMessageType(string outputMessageCode, OutputMessageType newOutputMessageType)
        {
            return _outputEnvelop.ChangeOutputMessageType(outputMessageCode, newOutputMessageType);
        }
        public OutputEnvelop ChangeOutputMessageDescription(string outputMessageCode, string newOutputMessageDescription)
        {
            return _outputEnvelop.ChangeOutputMessageDescription(outputMessageCode, newOutputMessageDescription);
        }
        public OutputEnvelop ChangeOutputMessageTypeAndOutputMessageDescription(string outputMessageCode, OutputMessageType newOutputMessageType, string newOutputMessageDescription)
        {
            return _outputEnvelop.ChangeOutputMessageTypeAndOutputMessageDescription(outputMessageCode, newOutputMessageType, newOutputMessageDescription);
        }

        public static OutputEnvelop Execute(Func<OutputEnvelop> handler)
        {
            return OutputEnvelop<object>.Execute(() => handler());
        }
        public static OutputEnvelop Execute<TInput>(TInput input, Func<TInput, OutputEnvelop> handler)
        {
            return OutputEnvelop<object>.Execute(input, i => handler(i));
        }

        public static async Task<OutputEnvelop> ExecuteAsync(Func<CancellationToken, Task<OutputEnvelop>> handler, CancellationToken cancellationToken)
        {
            return await OutputEnvelop<object>.ExecuteAsync(async c => await handler(c), cancellationToken);
        }
        public static async Task<OutputEnvelop> ExecuteAsync<TInput>(TInput input, Func<TInput, CancellationToken, Task<OutputEnvelop>> handler, CancellationToken cancellationToken)
        {
            return await OutputEnvelop<object>.ExecuteAsync(input, async (i, c) => await handler(i, c), cancellationToken);
        }

        // Builders
        public static OutputEnvelop Create<TOutput>(OutputEnvelop<TOutput> outputEnvelopWithOutput)
        {
            return new OutputEnvelop(outputEnvelopWithOutput.Type, outputEnvelopWithOutput.OutputMessageCollection, outputEnvelopWithOutput.ExceptionCollection);
        }
        public static OutputEnvelop Create(OutputEnvelopType type)
        {
            return OutputEnvelop<object>.Create(output: null, type, outputMessageCollection: null, exceptionCollection: null);
        }
        public static OutputEnvelop Create(OutputEnvelopType type, ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return OutputEnvelop<object>.Create(output: null, type, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop Create(OutputEnvelopType type, OutputEnvelop outputEnvelop)
        {
            return OutputEnvelop<object>.Create(output: null, type, outputEnvelop);
        }
        public static OutputEnvelop Create(OutputEnvelopType type, params OutputEnvelop[] outputEnvelopCollection)
        {
            var newOutputEnvelopCollection = new OutputEnvelop<object>[outputEnvelopCollection.Length];
            for (int i = 0; i < newOutputEnvelopCollection.Length; i++)
                newOutputEnvelopCollection[i] = outputEnvelopCollection[i];

            return OutputEnvelop<object>.Create(output: null, type, newOutputEnvelopCollection);
        }
        public static OutputEnvelop Create(OutputEnvelopType type, OutputMessageType outputMessageType, string outputMessageCode, string outputMessageDescription)
        {
            return Create(
                type,
                outputMessageCollection: new OutputMessage[]
                {
                    OutputMessage.Create(outputMessageType, outputMessageCode, outputMessageDescription)
                },
                exceptionCollection: Array.Empty<Exception>()
            );
        }
        public static OutputEnvelop Create(params OutputEnvelop[] outputEnvelopCollection)
        {
            var newOutputEnvelopCollection = new OutputEnvelop<object>[outputEnvelopCollection.Length];
            for (int i = 0; i < newOutputEnvelopCollection.Length; i++)
                newOutputEnvelopCollection[i] = outputEnvelopCollection[i];

            return OutputEnvelop<object>.Create(output: null, newOutputEnvelopCollection);
        }
        public static OutputEnvelop Create(OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
        {
            return OutputEnvelop<object>.Create(output: null, outputMessageCollection, exceptionCollection);
        }

        public static OutputEnvelop CreateSuccess()
        {
            return OutputEnvelop<object>.CreateSuccess(output: null);
        }
        public static OutputEnvelop CreateSuccess(ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return OutputEnvelop<object>.CreateSuccess(output: null, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreateSuccess(OutputEnvelop outputEnvelop)
        {
            return OutputEnvelop<object>.CreateSuccess(output: null, outputEnvelop);
        }
        public static OutputEnvelop CreateSuccess(params OutputEnvelop[] outputEnvelopCollection)
        {
            var newOutputEnvelopCollection = new OutputEnvelop<object>[outputEnvelopCollection.Length];
            for (int i = 0; i < outputEnvelopCollection.Length; i++)
                newOutputEnvelopCollection[i] = outputEnvelopCollection[i];

            return OutputEnvelop<object>.CreateSuccess(output: null, newOutputEnvelopCollection);
        }
        public static OutputEnvelop CreateSuccess(string outputMessageCode, string outputMessageDescription)
        {
            return CreateSuccess(
                outputMessageCollection: new OutputMessage[]
                {
                    OutputMessage.CreateSuccess(outputMessageCode, outputMessageDescription)
                },
                exceptionCollection: Array.Empty<Exception>()
            );
        }

        public static OutputEnvelop CreatePartial()
        {
            return OutputEnvelop<object>.CreatePartial(output: null);
        }
        public static OutputEnvelop CreatePartial(ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return OutputEnvelop<object>.CreatePartial(output: null, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreatePartial(OutputEnvelop outputEnvelop)
        {
            return OutputEnvelop<object>.CreatePartial(output: null, outputEnvelop);
        }
        public static OutputEnvelop CreatePartial(params OutputEnvelop[] outputEnvelopCollection)
        {
            var newOutputEnvelopCollection = new OutputEnvelop<object>[outputEnvelopCollection.Length];
            for (int i = 0; i < outputEnvelopCollection.Length; i++)
                newOutputEnvelopCollection[i] = outputEnvelopCollection[i];

            return OutputEnvelop<object>.CreatePartial(output: null, newOutputEnvelopCollection);
        }

        public static OutputEnvelop CreateError()
        {
            return OutputEnvelop<object>.CreateError(output: null);
        }
        public static OutputEnvelop CreateError(ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return OutputEnvelop<object>.CreateError(output: null, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop CreateError(OutputEnvelop outputEnvelop)
        {
            return OutputEnvelop<object>.CreateError(output: null, outputEnvelop);
        }
        public static OutputEnvelop CreateError(params OutputEnvelop[] outputEnvelopCollection)
        {
            var newOutputEnvelopCollection = new OutputEnvelop<object>[outputEnvelopCollection.Length];
            for (int i = 0; i < outputEnvelopCollection.Length; i++)
                newOutputEnvelopCollection[i] = outputEnvelopCollection[i];

            return OutputEnvelop<object>.CreateError(output: null, newOutputEnvelopCollection);
        }
        public static OutputEnvelop CreateError(string outputMessageCode, string outputMessageDescription)
        {
            return CreateError(
                outputMessageCollection: new OutputMessage[]
                {
                    OutputMessage.CreateError(outputMessageCode, outputMessageDescription)
                },
                exceptionCollection: Array.Empty<Exception>()
            );
        }
    }

    public readonly struct OutputEnvelop<TOutput>
    {
        // Properties
        public TOutput Output { get; }
        public OutputEnvelopType Type { get; }
        public ReadOnlyMemory<OutputMessage> OutputMessageCollection { get; }
        public ReadOnlyMemory<Exception> ExceptionCollection { get; }

        // Constructors
        private OutputEnvelop(
            TOutput output,
            OutputEnvelopType type,
            ReadOnlyMemory<OutputMessage> outputMessageCollection,
            ReadOnlyMemory<Exception> exceptionCollection
        )
        {
            Output = output;
            Type = type;
            OutputMessageCollection = outputMessageCollection;
            ExceptionCollection = exceptionCollection;
        }

        // Operators
        public static implicit operator OutputEnvelop<TOutput>(OutputEnvelop outputEnvelop)
        {
            return Create(output: default, outputEnvelop.Type, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }

        public static implicit operator OutputEnvelop(OutputEnvelop<TOutput> outputEnvelop)
        {
            return outputEnvelop.AsProcessResult();
        }

        // Public Methods
        public OutputEnvelop<TOutput> ChangeType(OutputEnvelopType type)
        {
            return new OutputEnvelop<TOutput>(Output, type, OutputMessageCollection, ExceptionCollection);
        }

        public OutputEnvelop<TOutput> AddOutputMessage(OutputMessage outputMessage)
        {
            return new OutputEnvelop<TOutput>(
                Output,
                Type,
                outputMessageCollection: ReadOnlyMemoryUtils.AddNewItem(OutputMessageCollection, outputMessage),
                ExceptionCollection
            );
        }
        public OutputEnvelop<TOutput> AddOutputMessageCollection(ReadOnlyMemory<OutputMessage> outputMessageCollection)
        {
            return new OutputEnvelop<TOutput>(
                Output,
                Type,
                outputMessageCollection: ReadOnlyMemoryUtils.AddRange(OutputMessageCollection, outputMessageCollection),
                ExceptionCollection
            );
        }

        public OutputEnvelop<TOutput> AddInformationOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateInformation(code, description));
        }
        public OutputEnvelop<TOutput> AddSuccessOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateSuccess(code, description));
        }
        public OutputEnvelop<TOutput> AddWarningOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateWarning(code, description));
        }
        public OutputEnvelop<TOutput> AddErrorOutputMessage(string code, string description)
        {
            return AddOutputMessage(OutputMessage.CreateError(code, description));
        }

        public OutputEnvelop<TOutput> AddException(Exception exception)
        {
            return new OutputEnvelop<TOutput>(
                Output,
                Type,
                OutputMessageCollection,
                exceptionCollection: ReadOnlyMemoryUtils.AddNewItem(ExceptionCollection, exception)
            );
        }
        public OutputEnvelop<TOutput> AddExceptionCollection(ReadOnlyMemory<Exception> exceptionCollection)
        {
            return new OutputEnvelop<TOutput>(
                Output,
                Type,
                OutputMessageCollection,
                exceptionCollection: ReadOnlyMemoryUtils.AddRange(ExceptionCollection, exceptionCollection)
            );
        }

        public OutputEnvelop<TOutput> ChangeOutputMessageType(string outputMessageCode, OutputMessageType newOutputMessageType)
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
                Output,
                Type, 
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(newOutputMessageCollection), 
                ExceptionCollection
            );
        }
        public OutputEnvelop<TOutput> ChangeOutputMessageDescription(string outputMessageCode, string newOutputMessageDescription)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection.Span[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeDescription(newOutputMessageDescription)
                    : outputMessage;
            }

            return Create(Output, Type, newOutputMessageCollection, ExceptionCollection);
        }
        public OutputEnvelop<TOutput> ChangeOutputMessageTypeAndOutputMessageDescription(string outputMessageCode, OutputMessageType newOutputMessageType, string newOutputMessageDescription)
        {
            var newOutputMessageCollection = new OutputMessage[OutputMessageCollection.Length];

            for (int i = 0; i < OutputMessageCollection.Length; i++)
            {
                var outputMessage = OutputMessageCollection.Span[i];

                newOutputMessageCollection[i] = string.Equals(outputMessage.Code, outputMessageCode, StringComparison.OrdinalIgnoreCase)
                    ? outputMessage.ChangeTypeAndDescription(newOutputMessageType, newOutputMessageDescription)
                    : outputMessage;
            }

            return Create( Output, Type, newOutputMessageCollection, ExceptionCollection);
        }

        public static OutputEnvelop<TOutput> Execute(Func<OutputEnvelop<TOutput>> handler)
        {
            try
            {
                return handler();
            }
            catch (Exception ex)
            {
                return Create(output: default, type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }
        public static OutputEnvelop<TOutput> Execute<TInput>(TInput input, Func<TInput, OutputEnvelop<TOutput>> handler)
        {
            try
            {
                return handler(input);
            }
            catch (Exception ex)
            {
                return Create(output: default, type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }

        public static async Task<OutputEnvelop<TOutput>> ExecuteAsync(Func<CancellationToken, Task<OutputEnvelop<TOutput>>> handler, CancellationToken cancellationToken)
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
                return Create(output: default, type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }
        public static async Task<OutputEnvelop<TOutput>> ExecuteAsync<TInput>(TInput input, Func<TInput, CancellationToken, Task<OutputEnvelop<TOutput>>> handler, CancellationToken cancellationToken)
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
                return Create(output: default, type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: new[] { ex });
            }
        }

        public OutputEnvelop AsProcessResult()
        {
            return OutputEnvelop.Create(this);
        }

        // Builders
        public static OutputEnvelop<TOutput> Create(TOutput output, OutputEnvelopType type)
        {
            return Create(output, type, outputMessageCollection: null, exceptionCollection: null);
        }

        public static OutputEnvelop<TOutput> Create(TOutput output, OutputEnvelopType type, ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            // Validate
            InvalidOutputEnvelopTypeException.ThrowIfInvalid(type);

            // Process and return
            return new OutputEnvelop<TOutput>(
                output,
                type, 
                outputMessageCollection,
                exceptionCollection
            );
        }
        public static OutputEnvelop<TOutput> Create(TOutput output, OutputEnvelopType type, OutputEnvelop<TOutput> outputEnvelop)
        {
            return new OutputEnvelop<TOutput>(output, type, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop<TOutput> Create(TOutput output, OutputEnvelopType type, params OutputEnvelop<TOutput>[] outputEnvelopCollection)
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
                output,
                type,
                outputMessageCollection: newMessageOutputCollection,
                exceptionCollection: newExceptionCollection
            );
        }
        public static OutputEnvelop<TOutput> Create(TOutput output, OutputEnvelopType type, OutputMessageType outputMessageType, string outputMessageCode, string outputMessageDescription)
        {
            return Create(
                output,
                type,
                outputMessageCollection: new OutputMessage[]
                {
                    OutputMessage.Create(outputMessageType, outputMessageCode, outputMessageDescription)
                },
                exceptionCollection: Array.Empty<Exception>()
            );
        }
        public static OutputEnvelop<TOutput> Create(TOutput output, params OutputEnvelop<TOutput>[] outputEnvelopCollection)
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

            return Create(output, type, outputEnvelopCollection);
        }
        public static OutputEnvelop<TOutput> Create(TOutput output, OutputMessage[] outputMessageCollection, Exception[] exceptionCollection)
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

            var type = hasSuccessMessageType
                ? hasErrorMessageType || hasException
                    ? OutputEnvelopType.Partial
                    : OutputEnvelopType.Success
                : hasErrorMessageType || hasException
                    ? OutputEnvelopType.Error
                    : OutputEnvelopType.Success;

            return Create(output, type, outputMessageCollection, exceptionCollection);
        }
        

        public static OutputEnvelop<TOutput> CreateSuccess(TOutput output)
        {
            return new OutputEnvelop<TOutput>(output, type: OutputEnvelopType.Success, outputMessageCollection: null, exceptionCollection: null);
        }
        public static OutputEnvelop<TOutput> CreateSuccess(TOutput output, ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return Create(output, type: OutputEnvelopType.Success, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop<TOutput> CreateSuccess(TOutput output, OutputEnvelop<TOutput> outputEnvelop)
        {
            return new OutputEnvelop<TOutput>(output, type: OutputEnvelopType.Success, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop<TOutput> CreateSuccess(TOutput output, params OutputEnvelop<TOutput>[] outputEnvelopCollection)
        {
            return Create(output, type: OutputEnvelopType.Success, outputEnvelopCollection);
        }
        public static OutputEnvelop<TOutput> CreateSuccess(TOutput output, string outputMessageCode, string outputMessageDescription)
        {
            return CreateSuccess(
                output,
                outputMessageCollection: new OutputMessage[]
                {
                    OutputMessage.CreateSuccess(outputMessageCode, outputMessageDescription)
                },
                exceptionCollection: Array.Empty<Exception>()
            );
        }

        public static OutputEnvelop<TOutput> CreatePartial(TOutput output)
        {
            return new OutputEnvelop<TOutput>(output, type: OutputEnvelopType.Partial, outputMessageCollection: null, exceptionCollection: null);
        }
        public static OutputEnvelop<TOutput> CreatePartial(TOutput output, ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return Create(output, type: OutputEnvelopType.Partial, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop<TOutput> CreatePartial(TOutput output, OutputEnvelop<TOutput> outputEnvelop)
        {
            return new OutputEnvelop<TOutput>(output, type: OutputEnvelopType.Partial, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop<TOutput> CreatePartial(TOutput output, params OutputEnvelop<TOutput>[] outputEnvelopCollection)
        {
            return Create(output, type: OutputEnvelopType.Partial, outputEnvelopCollection);
        }

        public static OutputEnvelop<TOutput> CreateError(TOutput output)
        {
            return new OutputEnvelop<TOutput>(output, type: OutputEnvelopType.Error, outputMessageCollection: null, exceptionCollection: null);
        }
        public static OutputEnvelop<TOutput> CreateError(TOutput output, ReadOnlyMemory<OutputMessage> outputMessageCollection, ReadOnlyMemory<Exception> exceptionCollection)
        {
            return Create(output, type: OutputEnvelopType.Error, outputMessageCollection, exceptionCollection);
        }
        public static OutputEnvelop<TOutput> CreateError(TOutput output, OutputEnvelop<TOutput> outputEnvelop)
        {
            return new OutputEnvelop<TOutput>(output, type: OutputEnvelopType.Error, outputEnvelop.OutputMessageCollection, outputEnvelop.ExceptionCollection);
        }
        public static OutputEnvelop<TOutput> CreateError(TOutput output, params OutputEnvelop<TOutput>[] outputEnvelopCollection)
        {
            return Create(output, type: OutputEnvelopType.Error, outputEnvelopCollection);
        }
        public static OutputEnvelop<TOutput> CreateError(TOutput output, string outputMessageCode, string outputMessageDescription)
        {
            return CreateError(
                output,
                outputMessageCollection: new OutputMessage[]
                {
                    OutputMessage.CreateError(outputMessageCode, outputMessageDescription)
                },
                exceptionCollection: Array.Empty<Exception>()
            );
        }
    }
}
