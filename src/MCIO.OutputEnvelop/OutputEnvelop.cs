using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputType;
using MCIO.OutputEnvelop.Models;
using MCIO.OutputEnvelop.Utils;

namespace MCIO.OutputEnvelop;

public readonly record struct OutputEnvelop
{
    // Properties
    public OutputType Type { get; }
    public OutputMessage[] OutputMessageCollection { get; }
    public Exception[] ExceptionCollection { get; }

    // Constructors
    private OutputEnvelop(
        OutputType type, 
        OutputMessage[]? outputMessageCollection,
        Exception[]? exceptionCollection
    )
    {
        Type = type;
        OutputMessageCollection = outputMessageCollection ?? [];
        ExceptionCollection = exceptionCollection ?? [];
    }

    // Public Methods
    public OutputEnvelop ChangeType(OutputType type) => new(type, OutputMessageCollection, ExceptionCollection);

    public OutputEnvelop AddOutputMessage(OutputMessage outputMessage)
    {
        return new(
            Type,
            outputMessageCollection: ArrayUtils.AddNewItem(OutputMessageCollection, outputMessage),
            ExceptionCollection
        );
    }
    public OutputEnvelop AddInformationOutputMessage(string code, string? description) => AddOutputMessage(OutputMessage.CreateInformation(code, description));
    public OutputEnvelop AddWarningOutputMessage(string code, string? description) => AddOutputMessage(OutputMessage.CreateWarning(code, description));
    public OutputEnvelop AddErrorOutputMessage(string code, string? description) => AddOutputMessage(OutputMessage.CreateError(code, description));
    public OutputEnvelop AddOutputMessageRange(OutputMessage[] outputMessageCollection)
    {
        return new(
            Type,
            outputMessageCollection: ArrayUtils.AddRange(OutputMessageCollection, outputMessageCollection),
            ExceptionCollection
        );
    }

    public OutputEnvelop AddException(Exception exception)
    {
        return new(
            Type,
            OutputMessageCollection,
            exceptionCollection: ArrayUtils.AddNewItem(ExceptionCollection, exception)
        );
    }
    public OutputEnvelop AddExceptionRange(Exception[] exceptionCollection)
    {
        return new(
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
    public OutputEnvelop ChangeOutputMessageDescription(string outputMessageCode, string? newOutputMessageDescription)
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
    public OutputEnvelop ChangeOutputMessageTypeAndOutputMessageDescription(string outputMessageCode, OutputMessageType newOutputMessageType, string? newOutputMessageDescription)
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
            return Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: [ex]);
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
            return Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: [ex]);
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
            return Task.FromResult(Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: [ex]));
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
            return Task.FromResult(Create(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: [ex]));
        }
    }

    // Builders
    public static OutputEnvelop Create(OutputType type) => new(type, outputMessageCollection: null, exceptionCollection: null);
    public static OutputEnvelop Create(OutputType type, OutputMessage[]? outputMessageCollection, Exception[]? exceptionCollection)
    {
        // Validate
        InvalidOutputTypeException.ThrowIfInvalid(type);

        // Process and return
        return new OutputEnvelop(
            type, 
            outputMessageCollection ?? [], 
            exceptionCollection ?? []
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

    public static OutputEnvelop CreateSuccess() => new(type: OutputType.Success, outputMessageCollection: null, exceptionCollection: null);
    public static OutputEnvelop CreateSuccess(OutputMessage[]? outputMessageCollection, Exception[]? exceptionCollection)
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

    public static OutputEnvelop CreatePartial() => new(type: OutputType.Partial, outputMessageCollection: null, exceptionCollection: null);
    public static OutputEnvelop CreatePartial(OutputMessage[]? outputMessageCollection, Exception[]? exceptionCollection)
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

    public static OutputEnvelop CreateError() => new(type: OutputType.Error, outputMessageCollection: null, exceptionCollection: null);
    public static OutputEnvelop CreateError(OutputMessage[]? outputMessageCollection, Exception[]? exceptionCollection)
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
