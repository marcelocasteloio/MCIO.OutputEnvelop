using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputMessageType;
using MCIO.OutputEnvelop.Models;

namespace MCIO.OutputEnvelop.UnitTests.ModelsTests;

public class OutputMessageTest
{
    [Theory]
    [InlineData(OutputMessageType.Information, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Information, "sample_code", null)]
    [InlineData(OutputMessageType.Warning, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Warning, "sample_code", null)]
    [InlineData(OutputMessageType.Success, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Success, "sample_code", null)]
    [InlineData(OutputMessageType.Error, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Error, "sample_code", null)]
    public void OutputMessage_Should_Created_From_Type(OutputMessageType type, string code, string? description)
    {
        // Act
        var outputMessage = OutputMessage.Create(type, code, description);

        // Assert
        outputMessage.Type.Should().Be(type);
        outputMessage.Code.Should().Be(code);
        outputMessage.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("sample_code", "sample_description")]
    [InlineData("sample_code", null)]
    public void OutputMessage_Should_Created_As_InformationType(string code, string? description)
    {
        // Arrange
        var expectedType = OutputMessageType.Information;

        // Act
        var outputMessage = OutputMessage.CreateInformation(code, description);

        // Assert
        outputMessage.Type.Should().Be(expectedType);
        outputMessage.Code.Should().Be(code);
        outputMessage.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("sample_code", "sample_description")]
    [InlineData("sample_code", null)]
    public void OutputMessage_Should_Created_As_SuccessType(string code, string? description)
    {
        // Arrange
        var expectedType = OutputMessageType.Success;

        // Act
        var outputMessage = OutputMessage.CreateSuccess(code, description);

        // Assert
        outputMessage.Type.Should().Be(expectedType);
        outputMessage.Code.Should().Be(code);
        outputMessage.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("sample_code", "sample_description")]
    [InlineData("sample_code", null)]
    public void OutputMessage_Should_Created_As_WarningType(string code, string? description)
    {
        // Arrange
        var expectedType = OutputMessageType.Warning;

        // Act
        var outputMessage = OutputMessage.CreateWarning(code, description);

        // Assert
        outputMessage.Type.Should().Be(expectedType);
        outputMessage.Code.Should().Be(code);
        outputMessage.Description.Should().Be(description);
    }

    [Theory]
    [InlineData("sample_code", "sample_description")]
    [InlineData("sample_code", null)]
    public void OutputMessage_Should_Created_As_ErrorType(string code, string? description)
    {
        // Arrange
        var expectedType = OutputMessageType.Error;

        // Act
        var outputMessage = OutputMessage.CreateError(code, description);

        // Assert
        outputMessage.Type.Should().Be(expectedType);
        outputMessage.Code.Should().Be(code);
        outputMessage.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(OutputMessageType.Information, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Information, "sample_code", null)]
    [InlineData(OutputMessageType.Warning, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Warning, "sample_code", null)]
    [InlineData(OutputMessageType.Success, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Success, "sample_code", null)]
    [InlineData(OutputMessageType.Error, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Error, "sample_code", null)]
    public void OutputMessage_Should_ChangeType(OutputMessageType type, string code, string? description)
    {
        // Arrange
        var expectedOutputMessageTypeCollection = Enum.GetValues<OutputMessageType>();
        var changedOutputMessageCollection = new OutputMessage[expectedOutputMessageTypeCollection.Length];

        var outputMessage = OutputMessage.Create(type, code, description);

        // Act
        for (int i = 0; i < expectedOutputMessageTypeCollection.Length; i++)
            changedOutputMessageCollection[i] = outputMessage.ChangeType(expectedOutputMessageTypeCollection[i]);

        // Assert
        for (int i = 0; i < expectedOutputMessageTypeCollection.Length; i++)
        {
            var changedOutputMessage = changedOutputMessageCollection[i];
            var expectedMessageType = expectedOutputMessageTypeCollection[i];

            changedOutputMessage.Should().NotBeSameAs(outputMessage);
            changedOutputMessage.Type.Should().Be(expectedMessageType);
            changedOutputMessage.Code.Should().Be(outputMessage.Code);
            changedOutputMessage.Description.Should().Be(outputMessage.Description);
        }
    }

    [Theory]
    [InlineData(OutputMessageType.Information, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Information, "sample_code", null)]
    [InlineData(OutputMessageType.Warning, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Warning, "sample_code", null)]
    [InlineData(OutputMessageType.Success, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Success, "sample_code", null)]
    [InlineData(OutputMessageType.Error, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Error, "sample_code", null)]
    public void OutputMessage_Should_ChangeDescription(OutputMessageType type, string code, string? description)
    {
        // Arrange
        var expectedNonNullableDescription = Guid.NewGuid().ToString();
        var expectedNullableDescription = (string?)null;

        var outputMessage = OutputMessage.Create(type, code, description);

        // Act
        var nonNulalbleDescriptionChangedOutputMessage = outputMessage.ChangeDescription(expectedNonNullableDescription);
        var nulalbleDescriptionChangedOutputMessage = outputMessage.ChangeDescription(expectedNullableDescription);

        // Assert
        nonNulalbleDescriptionChangedOutputMessage.Should().NotBeSameAs(outputMessage);
        nonNulalbleDescriptionChangedOutputMessage.Type.Should().Be(outputMessage.Type);
        nonNulalbleDescriptionChangedOutputMessage.Code.Should().Be(outputMessage.Code);
        nonNulalbleDescriptionChangedOutputMessage.Description.Should().Be(expectedNonNullableDescription);

        nulalbleDescriptionChangedOutputMessage.Should().NotBeSameAs(outputMessage);
        nulalbleDescriptionChangedOutputMessage.Type.Should().Be(outputMessage.Type);
        nulalbleDescriptionChangedOutputMessage.Code.Should().Be(outputMessage.Code);
        nulalbleDescriptionChangedOutputMessage.Description.Should().Be(expectedNullableDescription);
    }

    [Theory]
    [InlineData(OutputMessageType.Information, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Information, "sample_code", null)]
    [InlineData(OutputMessageType.Warning, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Warning, "sample_code", null)]
    [InlineData(OutputMessageType.Success, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Success, "sample_code", null)]
    [InlineData(OutputMessageType.Error, "sample_code", "sample_description")]
    [InlineData(OutputMessageType.Error, "sample_code", null)]
    public void OutputMessage_Should_ChangeTypeAndDescription(OutputMessageType type, string code, string? description)
    {
        // Arrange
        var expectedOutputMessageTypeCollection = Enum.GetValues<OutputMessageType>();
        var changedOutputMessageWithNonNullableDescriptionCollection = new OutputMessage[expectedOutputMessageTypeCollection.Length];
        var changedOutputMessageWithNullableDescriptionCollection = new OutputMessage[expectedOutputMessageTypeCollection.Length];

        var nonNullableDescription = Guid.NewGuid().ToString();
        var nullableDescription = (string?)null;

        var outputMessage = OutputMessage.Create(type, code, description);

        // Act
        for (int i = 0; i < expectedOutputMessageTypeCollection.Length; i++)
        {
            changedOutputMessageWithNonNullableDescriptionCollection[i] = outputMessage.ChangeTypeAndDescription(expectedOutputMessageTypeCollection[i], nonNullableDescription);
            changedOutputMessageWithNullableDescriptionCollection[i] = outputMessage.ChangeTypeAndDescription(expectedOutputMessageTypeCollection[i], nullableDescription);
        }

        // Assert
        for (int i = 0; i < expectedOutputMessageTypeCollection.Length; i++)
        {
            var expectedMessageType = expectedOutputMessageTypeCollection[i];
            var nonNullableChangedOutputMessage = changedOutputMessageWithNonNullableDescriptionCollection[i];
            var nullableChangedOutputMessage = changedOutputMessageWithNullableDescriptionCollection[i];

            nonNullableChangedOutputMessage.Should().NotBeSameAs(outputMessage);
            nonNullableChangedOutputMessage.Type.Should().Be(expectedMessageType);
            nonNullableChangedOutputMessage.Code.Should().Be(outputMessage.Code);
            nonNullableChangedOutputMessage.Description.Should().Be(nonNullableDescription);

            nullableChangedOutputMessage.Should().NotBeSameAs(outputMessage);
            nullableChangedOutputMessage.Type.Should().Be(expectedMessageType);
            nullableChangedOutputMessage.Code.Should().Be(outputMessage.Code);
            nullableChangedOutputMessage.Description.Should().Be(nullableDescription);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void OutputMessage_Should_Not_Created_With_NullOrWhiteSpace_Code(string? code)
    {
        // Act
        var actHandler = () =>
        {
            OutputMessage.CreateSuccess(code);
        };

        // Assert
        actHandler.Should().Throw<ArgumentNullException>().WithParameterName(nameof(code));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    public void OutputMessage_Should_Not_Created_With_Invalid_OutputMessageType(int outputMessageTypeInt)
    {
        // Arrange
        var outputMessageType = (OutputMessageType)outputMessageTypeInt;

        // Act
        var actHandler = () =>
        {
            OutputMessage.Create(outputMessageType, "sample_code");
        };

        // Assert
        actHandler.Should().Throw<InvalidOutputMessageTypeException>().Which.OutputMessageType.Should().Be(outputMessageType);
    }
}
