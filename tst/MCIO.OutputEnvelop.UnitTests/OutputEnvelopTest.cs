using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Models;

namespace MCIO.OutputEnvelop.UnitTests;

public class OutputEnvelopTest
{
    [Theory]
    [InlineData(OutputType.Error)]
    [InlineData(OutputType.Partial)]
    [InlineData(OutputType.Success)]
    public void OutputEvenelop_Should_Created(OutputType outputType)
    {
        // Act
        var outputEnvelop = OutputEnvelop.Create(outputType);

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().BeEmpty();
        outputEnvelop.ExceptionCollection.Should().BeEmpty();
    }

    [Theory]
    [InlineData(OutputType.Error)]
    [InlineData(OutputType.Partial)]
    [InlineData(OutputType.Success)]
    public void OutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection(OutputType outputType)
    {
        // Arrange
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.Create(
            outputType,
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var outputEnvelop = OutputEnvelop.Create(
            outputType,
            outputMessageCollection,
            exceptionCollection
        );

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().BeSameAs(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputType);
        outputEnvelopWithNullCollections.OutputMessageCollection.Should().BeEmpty();
        outputEnvelopWithNullCollections.ExceptionCollection.Should().BeEmpty();
    }

    [Theory]
    [InlineData(OutputType.Error)]
    [InlineData(OutputType.Partial)]
    [InlineData(OutputType.Success)]
    public void OutputEvenelop_Should_Created_From_Another_OutputEnvelop(OutputType outputType)
    {
        // Arrange
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };
        var existingOutputEnvelopWithNullCollections = OutputEnvelop.Create(
            outputType,
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var existingOutputEnvelop = OutputEnvelop.Create(
            outputType,
            outputMessageCollection,
            exceptionCollection
        );

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.Create(outputType, existingOutputEnvelopWithNullCollections);
        var outputEnvelop = OutputEnvelop.Create(outputType, existingOutputEnvelop);

        // Assert
        outputEnvelopWithNullCollections.Should().NotBeSameAs(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().NotBeSameAs(existingOutputEnvelop);
        outputEnvelopWithNullCollections.Should().Be(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().Be(existingOutputEnvelop);
    }

    [Theory]
    [InlineData(OutputType.Error)]
    [InlineData(OutputType.Partial)]
    [InlineData(OutputType.Success)]
    public void OutputEvenelop_Should_Created_From_OutputEnvelopCollection(OutputType outputType)
    {
        // Arrange
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.Create(outputType, existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.Should().BeSubsetOf(outputEnvelop.OutputMessageCollection);

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.Should().BeSubsetOf(outputEnvelop.ExceptionCollection);
        }
    }

    [Fact]
    public void SuccessOutputEvenelop_Should_Created()
    {
        // Act
        var expectedOutputType = OutputType.Success;
        var outputEnvelop = OutputEnvelop.CreateSuccess();

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputType);
        outputEnvelop.OutputMessageCollection.Should().BeEmpty();
        outputEnvelop.ExceptionCollection.Should().BeEmpty();
    }
    [Fact]
    public void SuccessOutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection()
    {
        // Arrange
        var outputType = OutputType.Success;
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection,
            exceptionCollection
        );

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().BeSameAs(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputType);
        outputEnvelopWithNullCollections.OutputMessageCollection.Should().BeEmpty();
        outputEnvelopWithNullCollections.ExceptionCollection.Should().BeEmpty();
    }
    [Fact]
    public void SuccessOutputEvenelop_Should_Created_From_Another_OutputEnvelop()
    {
        // Arrange
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };
        var existingOutputEnvelopWithNullCollections = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var existingOutputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection,
            exceptionCollection
        );

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.CreateSuccess(existingOutputEnvelopWithNullCollections);
        var outputEnvelop = OutputEnvelop.CreateSuccess(existingOutputEnvelop);

        // Assert
        outputEnvelopWithNullCollections.Should().NotBeSameAs(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().NotBeSameAs(existingOutputEnvelop);
        outputEnvelopWithNullCollections.Should().Be(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().Be(existingOutputEnvelop);
    }
    [Fact]
    public void SuccessOutputEvenelop_Should_Created_From_OutputEnvelopCollection()
    {
        // Arrange
        var outputType = OutputType.Success;
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.CreateSuccess(existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.Should().BeSubsetOf(outputEnvelop.OutputMessageCollection);

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.Should().BeSubsetOf(outputEnvelop.ExceptionCollection);
        }
    }

    [Fact]
    public void PartialOutputEvenelop_Should_Created()
    {
        // Act
        var expectedOutputType = OutputType.Partial;
        var outputEnvelop = OutputEnvelop.CreatePartial();

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputType);
        outputEnvelop.OutputMessageCollection.Should().BeEmpty();
        outputEnvelop.ExceptionCollection.Should().BeEmpty();
    }
    [Fact]
    public void PartialOutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection()
    {
        // Arrange
        var outputType = OutputType.Partial;
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.CreatePartial(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var outputEnvelop = OutputEnvelop.CreatePartial(
            outputMessageCollection,
            exceptionCollection
        );

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().BeSameAs(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputType);
        outputEnvelopWithNullCollections.OutputMessageCollection.Should().BeEmpty();
        outputEnvelopWithNullCollections.ExceptionCollection.Should().BeEmpty();
    }
    [Fact]
    public void PartialOutputEvenelop_Should_Created_From_Another_OutputEnvelop()
    {
        // Arrange
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };
        var existingOutputEnvelopWithNullCollections = OutputEnvelop.CreatePartial(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var existingOutputEnvelop = OutputEnvelop.CreatePartial(
            outputMessageCollection,
            exceptionCollection
        );

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.CreatePartial(existingOutputEnvelopWithNullCollections);
        var outputEnvelop = OutputEnvelop.CreatePartial(existingOutputEnvelop);

        // Assert
        outputEnvelopWithNullCollections.Should().NotBeSameAs(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().NotBeSameAs(existingOutputEnvelop);
        outputEnvelopWithNullCollections.Should().Be(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().Be(existingOutputEnvelop);
    }
    [Fact]
    public void PartialOutputEvenelop_Should_Created_From_OutputEnvelopCollection()
    {
        // Arrange
        var outputType = OutputType.Partial;
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.CreatePartial(existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.Should().BeSubsetOf(outputEnvelop.OutputMessageCollection);

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.Should().BeSubsetOf(outputEnvelop.ExceptionCollection);
        }
    }

    [Fact]
    public void ErrorOutputEvenelop_Should_Created()
    {
        // Act
        var expectedOutputType = OutputType.Error;
        var outputEnvelop = OutputEnvelop.CreateError();

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputType);
        outputEnvelop.OutputMessageCollection.Should().BeEmpty();
        outputEnvelop.ExceptionCollection.Should().BeEmpty();
    }
    [Fact]
    public void ErrorOutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection()
    {
        // Arrange
        var outputType = OutputType.Error;
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.CreateError(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var outputEnvelop = OutputEnvelop.CreateError(
            outputMessageCollection,
            exceptionCollection
        );

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().BeSameAs(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputType);
        outputEnvelopWithNullCollections.OutputMessageCollection.Should().BeEmpty();
        outputEnvelopWithNullCollections.ExceptionCollection.Should().BeEmpty();
    }
    [Fact]
    public void ErrorOutputEvenelop_Should_Created_From_Another_OutputEnvelop()
    {
        // Arrange
        var outputMessageCollection = new[]
        {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        };
        var exceptionCollection = new[]
        {
            new Exception()
        };
        var existingOutputEnvelopWithNullCollections = OutputEnvelop.CreateError(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var existingOutputEnvelop = OutputEnvelop.CreateError(
            outputMessageCollection,
            exceptionCollection
        );

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.CreateError(existingOutputEnvelopWithNullCollections);
        var outputEnvelop = OutputEnvelop.CreateError(existingOutputEnvelop);

        // Assert
        outputEnvelopWithNullCollections.Should().NotBeSameAs(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().NotBeSameAs(existingOutputEnvelop);
        outputEnvelopWithNullCollections.Should().Be(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().Be(existingOutputEnvelop);
    }
    [Fact]
    public void ErrorOutputEvenelop_Should_Created_From_OutputEnvelopCollection()
    {
        // Arrange
        var outputType = OutputType.Error;
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection:
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ],
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputType,
                outputMessageCollection: null,
                exceptionCollection:
                [
                    new Exception()
                ]
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.CreateError(existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputType);
        outputEnvelop.OutputMessageCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Should().HaveCount(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.Should().BeSubsetOf(outputEnvelop.OutputMessageCollection);

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.Should().BeSubsetOf(outputEnvelop.ExceptionCollection);
        }
    }

    [Theory]
    [InlineData(OutputType.Error)]
    [InlineData(OutputType.Success)]
    [InlineData(OutputType.Partial)]
    public void OutputEnvelop_Should_Change_Type(OutputType outputType)
    {
        // Arrange
        var outputMessageCollection = new[] { OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString()) };
        var exceptionCollection = new[] { new Exception() };
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var outputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];
        var changedOutputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];

        for (int i = 0; i < outputTypeCollection.Length; i++)
            outputEnvelopArray[i] = OutputEnvelop.Create(outputTypeCollection[i], outputMessageCollection, exceptionCollection);

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
            changedOutputEnvelopArray[i] = outputEnvelopArray[i].ChangeType(outputType);

        // Assert
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var changedOutputEnvelop = changedOutputEnvelopArray[i];

            changedOutputEnvelop.Type.Should().Be(outputType);
            changedOutputEnvelop.OutputMessageCollection.Should().BeSameAs(outputMessageCollection);
            changedOutputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);
        }
    }

    [Fact]
    public void OutputEnvelop_Should_AddOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var newOutputMessage = OutputMessage.CreateInformation(code: Guid.NewGuid().ToString());

        // Act
        var newOutputEnvelop = outputEnvelop.AddOutputMessage(newOutputMessage);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddOutputMessage(newOutputMessage);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(0);

        newOutputEnvelop.OutputMessageCollection.Should().HaveCount(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(1);

        newOutputEnvelop.OutputMessageCollection[1].Should().BeEquivalentTo(newOutputMessage);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Should().BeEquivalentTo(newOutputMessage);
    }

    [Fact]
    public void OutputEnvelop_Should_AddOutputMessageCollection()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var newOutputMessageCollection = new[] {
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
            OutputMessage.CreateError(code: Guid.NewGuid().ToString())
        };

        // Act
        var newOutputEnvelop = outputEnvelop.AddOutputMessageCollection(newOutputMessageCollection);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddOutputMessageCollection(newOutputMessageCollection);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(0);

        newOutputEnvelop.OutputMessageCollection.Should().HaveCount(3);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(2);

        newOutputMessageCollection.Should().BeSubsetOf(newOutputEnvelop.OutputMessageCollection);
        newOutputMessageCollection.Should().BeSubsetOf(newOutputEnvelopWithoutOutputMessage.OutputMessageCollection);
    }

    [Fact]
    public void OutputEnvelop_Should_AddInformationOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var expectedMessageType = OutputMessageType.Information;
        var messageCode = Guid.NewGuid().ToString();
        var messageDescription = Guid.NewGuid().ToString();

        // Act
        var newOutputEnvelop = outputEnvelop.AddInformationOutputMessage(messageCode, messageDescription);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddInformationOutputMessage(messageCode, messageDescription);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(0);

        newOutputEnvelop.OutputMessageCollection.Should().HaveCount(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(1);

        newOutputEnvelop.OutputMessageCollection[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddSuccessOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var expectedMessageType = OutputMessageType.Success;
        var messageCode = Guid.NewGuid().ToString();
        var messageDescription = Guid.NewGuid().ToString();

        // Act
        var newOutputEnvelop = outputEnvelop.AddSuccessOutputMessage(messageCode, messageDescription);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddSuccessOutputMessage(messageCode, messageDescription);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(0);

        newOutputEnvelop.OutputMessageCollection.Should().HaveCount(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(1);

        newOutputEnvelop.OutputMessageCollection[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddWarningOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var expectedMessageType = OutputMessageType.Warning;
        var messageCode = Guid.NewGuid().ToString();
        var messageDescription = Guid.NewGuid().ToString();

        // Act
        var newOutputEnvelop = outputEnvelop.AddWarningOutputMessage(messageCode, messageDescription);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddWarningOutputMessage(messageCode, messageDescription);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(0);

        newOutputEnvelop.OutputMessageCollection.Should().HaveCount(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(1);

        newOutputEnvelop.OutputMessageCollection[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddErrorOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var expectedMessageType = OutputMessageType.Error;
        var messageCode = Guid.NewGuid().ToString();
        var messageDescription = Guid.NewGuid().ToString();

        // Act
        var newOutputEnvelop = outputEnvelop.AddErrorOutputMessage(messageCode, messageDescription);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddErrorOutputMessage(messageCode, messageDescription);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(0);

        newOutputEnvelop.OutputMessageCollection.Should().HaveCount(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Should().HaveCount(1);

        newOutputEnvelop.OutputMessageCollection[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddException()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var newException = new Exception();

        // Act
        var newOutputEnvelop = outputEnvelop.AddException(newException);
        var newOutputEnvelopWithoutException = outputEnvelopWithoutOutputMessage.AddException(newException);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutException.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.ExceptionCollection.Should().HaveCount(1);
        outputEnvelopWithoutOutputMessage.ExceptionCollection.Should().HaveCount(0);

        newOutputEnvelop.ExceptionCollection.Should().HaveCount(2);
        newOutputEnvelopWithoutException.ExceptionCollection.Should().HaveCount(1);

        newOutputEnvelop.ExceptionCollection[1].Should().BeEquivalentTo(newException);
        newOutputEnvelopWithoutException.ExceptionCollection[0].Should().BeEquivalentTo(newException);
    }

    [Fact]
    public void OutputEnvelop_Should_AddExceptionCollection()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: [new Exception()]
        );
        var outputEnvelopWithoutException = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var newExceptionCollectionCollection = new[] {
            new Exception(Guid.NewGuid().ToString()),
            new Exception(Guid.NewGuid().ToString())
        };

        // Act
        var newOutputEnvelop = outputEnvelop.AddExceptionCollection(newExceptionCollectionCollection);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutException.AddExceptionCollection(newExceptionCollectionCollection);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutException);

        outputEnvelop.ExceptionCollection.Should().HaveCount(1);
        outputEnvelopWithoutException.ExceptionCollection.Should().HaveCount(0);

        newOutputEnvelop.ExceptionCollection.Should().HaveCount(3);
        newOutputEnvelopWithoutOutputMessage.ExceptionCollection.Should().HaveCount(2);

        newExceptionCollectionCollection.Should().BeSubsetOf(newOutputEnvelop.ExceptionCollection);
        newExceptionCollectionCollection.Should().BeSubsetOf(newOutputEnvelopWithoutOutputMessage.ExceptionCollection);
    }

    [Theory]
    [InlineData(OutputMessageType.Information)]
    [InlineData(OutputMessageType.Warning)]
    [InlineData(OutputMessageType.Error)]
    [InlineData(OutputMessageType.Success)]
    public void OutputEnvelop_Should_ChangeOutputMessageType(OutputMessageType outputMessageType)
    {
        // Arrange
        var firstOutputMessage = OutputMessage.Create(outputMessageType == OutputMessageType.Success ? OutputMessageType.Error : outputMessageType,  code: "1");
        var secondOutputMessage = OutputMessage.Create(outputMessageType == OutputMessageType.Success ? OutputMessageType.Error : outputMessageType, code: "2");
        var thirdOutputMessage = OutputMessage.Create(outputMessageType == OutputMessageType.Success ? OutputMessageType.Error : outputMessageType, code: "3");

        var outputMessageCollection = new[] {
            firstOutputMessage,
            secondOutputMessage,
            thirdOutputMessage,
        };
        var exceptionCollection = new[] { new Exception() };
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var outputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];
        var changedOutputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];

        for (int i = 0; i < outputTypeCollection.Length; i++)
            outputEnvelopArray[i] = OutputEnvelop.Create(outputTypeCollection[i], outputMessageCollection, exceptionCollection);

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
            changedOutputEnvelopArray[i] = outputEnvelopArray[i].ChangeOutputMessageType(secondOutputMessage.Code, outputMessageType);

        // Assert
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var changedOutputEnvelop = changedOutputEnvelopArray[i];

            changedOutputEnvelop.OutputMessageCollection.Should().HaveCount(outputMessageCollection.Length);
            changedOutputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);

            changedOutputEnvelop.OutputMessageCollection[0].Should().BeEquivalentTo(firstOutputMessage);
            changedOutputEnvelop.OutputMessageCollection[2].Should().BeEquivalentTo(thirdOutputMessage);

            changedOutputEnvelop.OutputMessageCollection[1].Type.Should().Be(outputMessageType);
        }

    }

    [Fact]
    public void OutputEnvelop_Should_ChangeOutputMessageDescription_From_NullableDescription()
    {
        // Arrange
        var nonNullableDescription = Guid.NewGuid().ToString();
        var nullableDescription = (string?)null;

        var nonNullableOutputMessageCode = Guid.NewGuid().ToString();
        var nullableOutputMessageCode = Guid.NewGuid().ToString();

        var outputMessageCollection = new[] {
            OutputMessage.Create(OutputMessageType.Success, code: nonNullableOutputMessageCode),
            OutputMessage.Create(OutputMessageType.Success, code: nullableOutputMessageCode)
        };
        var exceptionCollection = new[] { new Exception() };

        var outputEnvelop = OutputEnvelop.CreateSuccess(outputMessageCollection, exceptionCollection);

        // Act
        var newOutputEvenlopForNonNullableChange = outputEnvelop.ChangeOutputMessageDescription(nonNullableOutputMessageCode, nonNullableDescription);
        var newOutputEvenlopForNullableChange = outputEnvelop.ChangeOutputMessageDescription(nullableOutputMessageCode, nullableDescription);

        // Assert
        newOutputEvenlopForNonNullableChange.Should().NotBeEquivalentTo(outputEnvelop);
        newOutputEvenlopForNullableChange.Should().NotBeEquivalentTo(outputEnvelop);

        outputEnvelop.OutputMessageCollection[0].Description.Should().BeNull();
        outputEnvelop.OutputMessageCollection[1].Description.Should().BeNull();

        newOutputEvenlopForNonNullableChange.OutputMessageCollection.Should().HaveCount(2);
        newOutputEvenlopForNullableChange.OutputMessageCollection.Should().HaveCount(2);

        newOutputEvenlopForNonNullableChange.OutputMessageCollection[0].Description.Should().Be(nonNullableDescription);
        newOutputEvenlopForNullableChange.OutputMessageCollection[1].Description.Should().Be(nullableDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_ChangeOutputMessageDescription_From_NonNullableDescription()
    {
        // Arrange
        var nonNullableDescription = Guid.NewGuid().ToString();
        var nullableDescription = (string?)null;

        var nonNullableOutputMessageCode = Guid.NewGuid().ToString();
        var nullableOutputMessageCode = Guid.NewGuid().ToString();

        var outputMessageCollection = new[] {
            OutputMessage.Create(OutputMessageType.Success, code: nonNullableOutputMessageCode, description: Guid.NewGuid().ToString()),
            OutputMessage.Create(OutputMessageType.Success, code: nullableOutputMessageCode, description: Guid.NewGuid().ToString())
        };
        var exceptionCollection = new[] { new Exception() };

        var outputEnvelop = OutputEnvelop.CreateSuccess(outputMessageCollection, exceptionCollection);

        // Act
        var newOutputEvenlopForNonNullableChange = outputEnvelop.ChangeOutputMessageDescription(nonNullableOutputMessageCode, nonNullableDescription);
        var newOutputEvenlopForNullableChange = outputEnvelop.ChangeOutputMessageDescription(nullableOutputMessageCode, nullableDescription);

        // Assert
        newOutputEvenlopForNonNullableChange.Should().NotBeEquivalentTo(outputEnvelop);
        newOutputEvenlopForNullableChange.Should().NotBeEquivalentTo(outputEnvelop);

        outputEnvelop.OutputMessageCollection[0].Description.Should().NotBeNull();
        outputEnvelop.OutputMessageCollection[1].Description.Should().NotBeNull();

        newOutputEvenlopForNonNullableChange.OutputMessageCollection.Should().HaveCount(2);
        newOutputEvenlopForNullableChange.OutputMessageCollection.Should().HaveCount(2);

        newOutputEvenlopForNonNullableChange.OutputMessageCollection[0].Description.Should().Be(nonNullableDescription);
        newOutputEvenlopForNullableChange.OutputMessageCollection[1].Description.Should().Be(nullableDescription);
    }

    [Theory]
    [InlineData(OutputMessageType.Information, "description for information")]
    [InlineData(OutputMessageType.Information, null)]
    [InlineData(OutputMessageType.Warning, "description for warning")]
    [InlineData(OutputMessageType.Warning, null)]
    [InlineData(OutputMessageType.Error, "description for error")]
    [InlineData(OutputMessageType.Error, null)]
    [InlineData(OutputMessageType.Success, "description for success")]
    [InlineData(OutputMessageType.Success, null)]
    public void OutputEnvelop_Should_ChangeOutputMessageDescription(OutputMessageType outputMessageType, string? description)
    {
        // Arrange
        var firstOutputMessage = OutputMessage.Create(outputMessageType == OutputMessageType.Success ? OutputMessageType.Error : outputMessageType, code: "1");
        var secondOutputMessage = OutputMessage.Create(outputMessageType == OutputMessageType.Success ? OutputMessageType.Error : outputMessageType, code: "2");
        var thirdOutputMessage = OutputMessage.Create(outputMessageType == OutputMessageType.Success ? OutputMessageType.Error : outputMessageType, code: "3");

        var outputMessageCollection = new[] {
            firstOutputMessage,
            secondOutputMessage,
            thirdOutputMessage,
        };
        var exceptionCollection = new[] { new Exception() };
        var outputTypeCollection = Enum.GetValues<OutputType>();
        var outputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];
        var changedOutputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];

        for (int i = 0; i < outputTypeCollection.Length; i++)
            outputEnvelopArray[i] = OutputEnvelop.Create(outputTypeCollection[i], outputMessageCollection, exceptionCollection);

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
            changedOutputEnvelopArray[i] = outputEnvelopArray[i].ChangeOutputMessageTypeAndOutputMessageDescription(secondOutputMessage.Code, outputMessageType, description);

        // Assert
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var changedOutputEnvelop = changedOutputEnvelopArray[i];

            changedOutputEnvelop.OutputMessageCollection.Should().HaveCount(outputMessageCollection.Length);
            changedOutputEnvelop.ExceptionCollection.Should().BeSameAs(exceptionCollection);

            changedOutputEnvelop.OutputMessageCollection[0].Should().BeEquivalentTo(firstOutputMessage);
            changedOutputEnvelop.OutputMessageCollection[2].Should().BeEquivalentTo(thirdOutputMessage);

            changedOutputEnvelop.OutputMessageCollection[1].Type.Should().Be(outputMessageType);
            changedOutputEnvelop.OutputMessageCollection[1].Description.Should().Be(description);
        }
    }
}
