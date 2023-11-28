using MCIO.OutputEnvelop.Enums;
using MCIO.OutputEnvelop.Exceptions.InvalidOutputEnvelopType;
using MCIO.OutputEnvelop.Models;

namespace MCIO.OutputEnvelop.UnitTests;

public class OutputEnvelopTest
{
    [Theory]
    [InlineData(OutputEnvelopType.Error)]
    [InlineData(OutputEnvelopType.Partial)]
    [InlineData(OutputEnvelopType.Success)]
    public void OutputEvenelop_Should_Created(OutputEnvelopType outputEnvelopType)
    {
        // Act
        var outputEnvelop = OutputEnvelop.Create(outputEnvelopType);

        // Assert
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
    }

    [Theory]
    [InlineData(OutputEnvelopType.Error)]
    [InlineData(OutputEnvelopType.Partial)]
    [InlineData(OutputEnvelopType.Success)]
    public void OutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection(OutputEnvelopType outputEnvelopType)
    {
        // Arrange
        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>(
        [
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        ]);
        var exceptionCollection = new ReadOnlyMemory<Exception>(
        [
            new Exception()
        ]);

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.Create(
            outputEnvelopType,
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var outputEnvelop = OutputEnvelop.Create(
            outputEnvelopType,
            outputMessageCollection,
            exceptionCollection
        );

        // Assert
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Should().BeEquivalentTo(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputEnvelopType);
        outputEnvelopWithNullCollections.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelopWithNullCollections.ExceptionCollection.IsEmpty.Should().BeTrue();
    }

    [Theory]
    [InlineData(OutputEnvelopType.Error)]
    [InlineData(OutputEnvelopType.Partial)]
    [InlineData(OutputEnvelopType.Success)]
    public void OutputEvenelop_Should_Created_From_Another_OutputEnvelop(OutputEnvelopType outputEnvelopType)
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
            outputEnvelopType,
            outputMessageCollection: null,
            exceptionCollection: null
        );
        var existingOutputEnvelop = OutputEnvelop.Create(
            outputEnvelopType,
            outputMessageCollection,
            exceptionCollection
        );

        // Act
        var outputEnvelopWithNullCollections = OutputEnvelop.Create(outputEnvelopType, existingOutputEnvelopWithNullCollections);
        var outputEnvelop = OutputEnvelop.Create(outputEnvelopType, existingOutputEnvelop);

        // Assert
        outputEnvelopWithNullCollections.Should().NotBeSameAs(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().NotBeSameAs(existingOutputEnvelop);
        outputEnvelopWithNullCollections.Should().Be(existingOutputEnvelopWithNullCollections);
        outputEnvelop.Should().Be(existingOutputEnvelop);
    }

    [Theory]
    [InlineData(OutputEnvelopType.Error)]
    [InlineData(OutputEnvelopType.Partial)]
    [InlineData(OutputEnvelopType.Success)]
    public void OutputEvenelop_Should_Created_From_OutputEnvelopCollection(OutputEnvelopType outputEnvelopType)
    {
        // Arrange
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.Create(outputEnvelopType, existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.ToArray().Should().BeSubsetOf(outputEnvelop.OutputMessageCollection.ToArray());

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.ToArray().Should().BeSubsetOf(outputEnvelop.ExceptionCollection.ToArray());
        }
    }

    [Fact]
    public void SuccessOutputEvenelop_Should_Created()
    {
        // Act
        var expectedOutputType = OutputEnvelopType.Success;
        var outputEnvelop = OutputEnvelop.CreateSuccess();

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputType);
        outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
    }
    [Fact]
    public void SuccessOutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection()
    {
        // Arrange
        var outputEnvelopType = OutputEnvelopType.Success;
        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>(
        [
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        ]);
        var exceptionCollection = new ReadOnlyMemory<Exception>(
        [
            new Exception()
        ]);

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
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Should().BeEquivalentTo(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputEnvelopType);
        outputEnvelopWithNullCollections.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelopWithNullCollections.ExceptionCollection.IsEmpty.Should().BeTrue();
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
        var outputEnvelopType = OutputEnvelopType.Success;
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.CreateSuccess(existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.ToArray().Should().BeSubsetOf(outputEnvelop.OutputMessageCollection.ToArray());

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.ToArray().Should().BeSubsetOf(outputEnvelop.ExceptionCollection.ToArray());
        }
    }

    [Fact]
    public void PartialOutputEvenelop_Should_Created()
    {
        // Act
        var expectedOutputType = OutputEnvelopType.Partial;
        var outputEnvelop = OutputEnvelop.CreatePartial();

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputType);
        outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
    }
    [Fact]
    public void PartialOutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection()
    {
        // Arrange
        var outputEnvelopType = OutputEnvelopType.Partial;
        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>(
        [
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        ]);
        var exceptionCollection = new ReadOnlyMemory<Exception>(
        [
            new Exception()
        ]);

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
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Should().BeEquivalentTo(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputEnvelopType);
        outputEnvelopWithNullCollections.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelopWithNullCollections.ExceptionCollection.IsEmpty.Should().BeTrue();
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
        var outputEnvelopType = OutputEnvelopType.Partial;
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.CreatePartial(existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.ToArray().Should().BeSubsetOf(outputEnvelop.OutputMessageCollection.ToArray());

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.ToArray().Should().BeSubsetOf(outputEnvelop.ExceptionCollection.ToArray());
        }
    }

    [Fact]
    public void ErrorOutputEvenelop_Should_Created()
    {
        // Act
        var expectedOutputType = OutputEnvelopType.Error;
        var outputEnvelop = OutputEnvelop.CreateError();

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputType);
        outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
    }
    [Fact]
    public void ErrorOutputEvenelop_Should_Created_WithOutputMessageCollection_And_ExceptionCollection()
    {
        // Arrange
        var outputEnvelopType = OutputEnvelopType.Error;
        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>(
        [
            OutputMessage.CreateInformation(code: Guid.NewGuid().ToString(), description: null)
        ]);
        var exceptionCollection = new ReadOnlyMemory<Exception>(
        [
            new Exception()
        ]);

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
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Should().BeEquivalentTo(outputMessageCollection);
        outputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);

        outputEnvelopWithNullCollections.Type.Should().Be(outputEnvelopType);
        outputEnvelopWithNullCollections.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelopWithNullCollections.ExceptionCollection.IsEmpty.Should().BeTrue();
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
        var outputEnvelopType = OutputEnvelopType.Error;
        var existingOutputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: new ReadOnlyMemory<OutputMessage>(
                [
                    OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
                    OutputMessage.CreateWarning(code: Guid.NewGuid().ToString())
                ]),
                exceptionCollection: null
            ),
            OutputEnvelop.Create(
                outputEnvelopType,
                outputMessageCollection: null,
                exceptionCollection: new ReadOnlyMemory<Exception>(
                [
                    new Exception()
                ])
            ),
        };

        // Act
        var outputEnvelop = OutputEnvelop.CreateError(existingOutputEnvelopCollection);

        // Assert
        outputEnvelop.Type.Should().Be(outputEnvelopType);
        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.OutputMessageCollection.Length));
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(existingOutputEnvelopCollection.Sum(q => q.ExceptionCollection.Length));

        foreach (var existingOutputEnvelop in existingOutputEnvelopCollection)
        {
            if (existingOutputEnvelop.OutputMessageCollection.Length > 0)
                existingOutputEnvelop.OutputMessageCollection.ToArray().Should().BeSubsetOf(outputEnvelop.OutputMessageCollection.ToArray());

            if (existingOutputEnvelop.ExceptionCollection.Length > 0)
                existingOutputEnvelop.ExceptionCollection.ToArray().Should().BeSubsetOf(outputEnvelop.ExceptionCollection.ToArray());
        }
    }

    [Theory]
    [InlineData(OutputEnvelopType.Error)]
    [InlineData(OutputEnvelopType.Success)]
    [InlineData(OutputEnvelopType.Partial)]
    public void OutputEnvelop_Should_Change_Type(OutputEnvelopType outputEnvelopType)
    {
        // Arrange
        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]);
        var exceptionCollection = new ReadOnlyMemory<Exception>([new Exception()]);
        var outputTypeCollection = Enum.GetValues<OutputEnvelopType>();
        var outputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];
        var changedOutputEnvelopArray = new OutputEnvelop[outputTypeCollection.Length];

        for (int i = 0; i < outputTypeCollection.Length; i++)
            outputEnvelopArray[i] = OutputEnvelop.Create(outputTypeCollection[i], outputMessageCollection, exceptionCollection);

        // Act
        for (int i = 0; i < outputTypeCollection.Length; i++)
            changedOutputEnvelopArray[i] = outputEnvelopArray[i].ChangeType(outputEnvelopType);

        // Assert
        for (int i = 0; i < outputTypeCollection.Length; i++)
        {
            var changedOutputEnvelop = changedOutputEnvelopArray[i];

            changedOutputEnvelop.Type.Should().Be(outputEnvelopType);
            changedOutputEnvelop.OutputMessageCollection.Should().BeEquivalentTo(outputMessageCollection);
            changedOutputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);
        }
    }

    [Fact]
    public void OutputEnvelop_Should_AddOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]),
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var newOutputMessage = OutputMessage.CreateInformation(code: Guid.NewGuid().ToString());

        // Act
        var newOutputEnvelop = outputEnvelop.AddOutputMessage(newOutputMessage);
        var newOutputEnvelopWithoutOutputMessage = outputEnvelopWithoutOutputMessage.AddOutputMessage(newOutputMessage);

        // Assert
        newOutputEnvelop.Should().NotBeSameAs(outputEnvelop);
        newOutputEnvelopWithoutOutputMessage.Should().NotBeSameAs(outputEnvelopWithoutOutputMessage);

        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(1);

        newOutputEnvelop.OutputMessageCollection.Span[1].Should().BeEquivalentTo(newOutputMessage);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Should().BeEquivalentTo(newOutputMessage);
    }

    [Fact]
    public void OutputEnvelop_Should_AddOutputMessageCollection()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]),
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(3);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(2);

        newOutputMessageCollection.Should().BeSubsetOf(newOutputEnvelop.OutputMessageCollection.ToArray());
        newOutputMessageCollection.Should().BeSubsetOf(newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.ToArray());
    }

    [Fact]
    public void OutputEnvelop_Should_AddInformationOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]),
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(1);

        newOutputEnvelop.OutputMessageCollection.Span[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection.Span[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection.Span[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddSuccessOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]),
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(1);

        newOutputEnvelop.OutputMessageCollection.Span[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection.Span[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection.Span[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddWarningOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]),
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(1);

        newOutputEnvelop.OutputMessageCollection.Span[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection.Span[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection.Span[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddErrorOutputMessage()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: new ReadOnlyMemory<OutputMessage>([OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())]),
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
        );
        var outputEnvelopWithoutOutputMessage = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.OutputMessageCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span.Length.Should().Be(1);

        newOutputEnvelop.OutputMessageCollection.Span[1].Type.Should().Be(expectedMessageType);
        newOutputEnvelop.OutputMessageCollection.Span[1].Code.Should().Be(messageCode);
        newOutputEnvelop.OutputMessageCollection.Span[1].Description.Should().Be(messageDescription);

        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Type.Should().Be(expectedMessageType);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Code.Should().Be(messageCode);
        newOutputEnvelopWithoutOutputMessage.OutputMessageCollection.Span[0].Description.Should().Be(messageDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_AddException()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutOutputMessage.ExceptionCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.ExceptionCollection.Span.Length.Should().Be(2);
        newOutputEnvelopWithoutException.ExceptionCollection.Span.Length.Should().Be(1);

        newOutputEnvelop.ExceptionCollection.Span[1].Should().BeEquivalentTo(newException);
        newOutputEnvelopWithoutException.ExceptionCollection.Span[0].Should().BeEquivalentTo(newException);
    }

    [Fact]
    public void OutputEnvelop_Should_AddExceptionCollection()
    {
        // Arrange
        var outputEnvelop = OutputEnvelop.CreateSuccess(
            outputMessageCollection: null,
            exceptionCollection: new ReadOnlyMemory<Exception>([new Exception()])
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

        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(1);
        outputEnvelopWithoutException.ExceptionCollection.Span.Length.Should().Be(0);

        newOutputEnvelop.ExceptionCollection.Span.Length.Should().Be(3);
        newOutputEnvelopWithoutOutputMessage.ExceptionCollection.Span.Length.Should().Be(2);

        newExceptionCollectionCollection.Should().BeSubsetOf(newOutputEnvelop.ExceptionCollection.ToArray());
        newExceptionCollectionCollection.Should().BeSubsetOf(newOutputEnvelopWithoutOutputMessage.ExceptionCollection.ToArray());
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

        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>([
            firstOutputMessage,
            secondOutputMessage,
            thirdOutputMessage,
        ]);
        var exceptionCollection = new ReadOnlyMemory<Exception>([new Exception()]);
        var outputTypeCollection = Enum.GetValues<OutputEnvelopType>();
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

            changedOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(outputMessageCollection.Length);
            changedOutputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);

            changedOutputEnvelop.OutputMessageCollection.Span[0].Should().BeEquivalentTo(firstOutputMessage);
            changedOutputEnvelop.OutputMessageCollection.Span[2].Should().BeEquivalentTo(thirdOutputMessage);

            changedOutputEnvelop.OutputMessageCollection.Span[1].Type.Should().Be(outputMessageType);
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

        outputEnvelop.OutputMessageCollection.Span[0].Description.Should().BeNull();
        outputEnvelop.OutputMessageCollection.Span[1].Description.Should().BeNull();

        newOutputEvenlopForNonNullableChange.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEvenlopForNullableChange.OutputMessageCollection.Span.Length.Should().Be(2);

        newOutputEvenlopForNonNullableChange.OutputMessageCollection.Span[0].Description.Should().Be(nonNullableDescription);
        newOutputEvenlopForNullableChange.OutputMessageCollection.Span[1].Description.Should().Be(nullableDescription);
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

        outputEnvelop.OutputMessageCollection.Span[0].Description.Should().NotBeNull();
        outputEnvelop.OutputMessageCollection.Span[1].Description.Should().NotBeNull();

        newOutputEvenlopForNonNullableChange.OutputMessageCollection.Span.Length.Should().Be(2);
        newOutputEvenlopForNullableChange.OutputMessageCollection.Span.Length.Should().Be(2);

        newOutputEvenlopForNonNullableChange.OutputMessageCollection.Span[0].Description.Should().Be(nonNullableDescription);
        newOutputEvenlopForNullableChange.OutputMessageCollection.Span[1].Description.Should().Be(nullableDescription);
    }

    [Fact]
    public void OutputEnvelop_Should_Created_Success_FromExistingOutputEnvelopCollection()
    {
        // Arrange
        var expectedOutputEnvelopType = OutputEnvelopType.Success;
        var outputEnvelopCollectionA = new[] {
            OutputEnvelop.CreateSuccess()
        };

        // Act
        var outputEnvelop = OutputEnvelop.Create(outputEnvelopCollectionA);

        // Assert
        outputEnvelop.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void OutputEnvelop_Should_Created_Partial_FromExistingOutputEnvelopCollection()
    {
        // Arrange
        var expectedOutputEnvelopType = OutputEnvelopType.Partial;
        var outputEnvelopCollectionA = new[] {
            OutputEnvelop.CreateSuccess(),
            OutputEnvelop.CreateError()
        };
        var outputEnvelopCollectionB = new[] {
            OutputEnvelop.CreateSuccess(),
            OutputEnvelop.CreateError(),
            OutputEnvelop.CreatePartial()
        };
        var outputEnvelopCollectionC = new[] {
            OutputEnvelop.CreatePartial(),
            OutputEnvelop.CreatePartial()
        };

        // Act
        var outputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(outputEnvelopCollectionA),
            OutputEnvelop.Create(outputEnvelopCollectionB),
            OutputEnvelop.Create(outputEnvelopCollectionC)
        };

        // Assert
        foreach (var outputEnvelop in outputEnvelopCollection)
        {
            outputEnvelop.Type.Should().Be(expectedOutputEnvelopType);
            outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
            outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
        }
    }

    [Fact]
    public void OutputEnvelop_Should_Created_Error_FromExistingOutputEnvelopCollection()
    {
        // Arrange
        var expectedOutputEnvelopType = OutputEnvelopType.Error;
        var outputEnvelopCollectionA = new[] {
            OutputEnvelop.CreateError(),
            OutputEnvelop.CreateError()
        };

        // Act
        var outputEnvelopCollection = new[]
        {
            OutputEnvelop.Create(outputEnvelopCollectionA)
        };

        // Assert
        foreach (var outputEnvelop in outputEnvelopCollection)
        {
            outputEnvelop.Type.Should().Be(expectedOutputEnvelopType);
            outputEnvelop.OutputMessageCollection.IsEmpty.Should().BeTrue();
            outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
        }
    }

    [Fact]
    public void OutputEnvelop_Should_Created_Success_FromMessageCollectionAndExceptionCollection()
    {
        // Arrange
        var expectedOutputEnvelopType = OutputEnvelopType.Success;

        var outputMessageCollectionArray = new[]
        {
            [OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())],
            [OutputMessage.CreateInformation(code: Guid.NewGuid().ToString())],
            [
                OutputMessage.CreateInformation(code: Guid.NewGuid().ToString()),
                OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())
            ],
            Array.Empty<OutputMessage>()
        };
        var outputEnvelopCollection = new OutputEnvelop[outputMessageCollectionArray.Length];

        // Act
        for (int i = 0; i < outputEnvelopCollection.Length; i++)
        {
            outputEnvelopCollection[i] = OutputEnvelop.Create(
                outputMessageCollectionArray[i],
                exceptionCollection: []
            );
        }

        // Assert
        for (int i = 0; i < outputEnvelopCollection.Length; i++)
        {
            var outputEnvelop = outputEnvelopCollection[i];

            outputEnvelop.Type.Should().Be(expectedOutputEnvelopType);
            outputEnvelop.OutputMessageCollection.ToArray().Should().BeEquivalentTo(outputMessageCollectionArray[i]);
            outputEnvelop.ExceptionCollection.IsEmpty.Should().BeTrue();
        }
    }

    [Fact]
    public void OutputEnvelop_Should_Created_Partial_FromMessageCollectionAndExceptionCollection()
    {
        // Arrange
        var expectedOutputEnvelopType = OutputEnvelopType.Partial;

        var outputMessageCollectionA = new[]{
            OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString()),
            OutputMessage.CreateError(code: Guid.NewGuid().ToString()),
        };
        var outputMessageCollectionB = new[]{
            OutputMessage.CreateSuccess(code: Guid.NewGuid().ToString())
        };
        var exceptionCollection =  new[] { new Exception() };

        // Act
        var outputEnvelopA = OutputEnvelop.Create(outputMessageCollectionA, exceptionCollection: []);
        var outputEnvelopB = OutputEnvelop.Create(outputMessageCollectionB, exceptionCollection);
        var outputEnvelopC = OutputEnvelop.Create(outputMessageCollectionA, exceptionCollection);

        // Assert
        outputEnvelopA.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelopA.OutputMessageCollection.ToArray().Should().BeEquivalentTo(outputMessageCollectionA);
        outputEnvelopA.ExceptionCollection.IsEmpty.Should().BeTrue();

        outputEnvelopB.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelopB.OutputMessageCollection.ToArray().Should().BeEquivalentTo(outputMessageCollectionB);
        outputEnvelopB.ExceptionCollection.ToArray().Should().BeEquivalentTo(exceptionCollection);

        outputEnvelopC.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelopC.OutputMessageCollection.ToArray().Should().BeEquivalentTo(outputMessageCollectionA);
        outputEnvelopC.ExceptionCollection.ToArray().Should().BeEquivalentTo(exceptionCollection);
    }

    [Fact]
    public void OutputEnvelop_Should_Created_Error_FromMessageCollectionAndExceptionCollection()
    {
        // Arrange
        var expectedOutputEnvelopType = OutputEnvelopType.Error;

        var outputMessageCollection = new[]{
            OutputMessage.CreateError(code: Guid.NewGuid().ToString())
        };
        var exceptionCollection = new[] { new Exception() };

        // Act
        var outputEnvelopA = OutputEnvelop.Create(outputMessageCollection, exceptionCollection: []);
        var outputEnvelopB = OutputEnvelop.Create(outputMessageCollection, exceptionCollection);
        var outputEnvelopC = OutputEnvelop.Create(outputMessageCollection: [], exceptionCollection);

        // Assert
        outputEnvelopA.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelopA.OutputMessageCollection.ToArray().Should().BeEquivalentTo(outputMessageCollection);
        outputEnvelopA.ExceptionCollection.IsEmpty.Should().BeTrue();

        outputEnvelopB.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelopB.OutputMessageCollection.ToArray().Should().BeEquivalentTo(outputMessageCollection);
        outputEnvelopB.ExceptionCollection.ToArray().Should().BeEquivalentTo(exceptionCollection);

        outputEnvelopC.Type.Should().Be(expectedOutputEnvelopType);
        outputEnvelopC.OutputMessageCollection.IsEmpty.Should().BeTrue();
        outputEnvelopC.ExceptionCollection.ToArray().Should().BeEquivalentTo(exceptionCollection);
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

        var outputMessageCollection = new ReadOnlyMemory<OutputMessage>([
            firstOutputMessage,
            secondOutputMessage,
            thirdOutputMessage,
        ]);
        var exceptionCollection = new ReadOnlyMemory<Exception>([new Exception()]);
        var outputTypeCollection = Enum.GetValues<OutputEnvelopType>();
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

            changedOutputEnvelop.OutputMessageCollection.Span.Length.Should().Be(outputMessageCollection.Length);
            changedOutputEnvelop.ExceptionCollection.Should().BeEquivalentTo(exceptionCollection);

            changedOutputEnvelop.OutputMessageCollection.Span[0].Should().BeEquivalentTo(firstOutputMessage);
            changedOutputEnvelop.OutputMessageCollection.Span[2].Should().BeEquivalentTo(thirdOutputMessage);

            changedOutputEnvelop.OutputMessageCollection.Span[1].Type.Should().Be(outputMessageType);
            changedOutputEnvelop.OutputMessageCollection.Span[1].Description.Should().Be(description);
        }
    }

    [Fact]
    public void OutputEnvelop_Should_Successfull_Executed()
    {
        // Arrange
        var id = Guid.Empty;

        // Act
        var outputEnvelop = OutputEnvelop.Execute(
            handler: () =>
            {
                id = Guid.NewGuid();

                return OutputEnvelop.CreateSuccess();
            }
        );

        // Assert
        id.Should().NotBe(default(Guid));
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Success);
    }

    [Fact]
    public void OutputEnvelop_Should_Executed_TrowingException()
    {
        // Arrange
        var id = Guid.Empty;
        var exceptionToTrow = new Exception();

        // Act
        var outputEnvelop = OutputEnvelop.Execute(
            handler: () =>
            {
                throw exceptionToTrow;
#pragma warning disable CS0162 // Unreachable code detected
                id = Guid.NewGuid();
                return OutputEnvelop.CreateSuccess();
#pragma warning restore CS0162 // Unreachable code detected
            }
        );

        // Assert
        id.Should().Be(default(Guid));
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Error);
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(1);
        outputEnvelop.ExceptionCollection.ToArray()[0].Should().Be(exceptionToTrow);
    }

    [Fact]
    public void OutputEnvelop_Should_Successfull_ExecutedWithInput()
    {
        // Arrange
        var customer = new {
            Id = 1
        };
        var processedCustomer = (object?)null;

        // Act
        var outputEnvelop = OutputEnvelop.Execute(
            input: customer,
            handler: input =>
            {
                processedCustomer = input;

                return OutputEnvelop.CreateSuccess();
            }
        );

        // Assert
        processedCustomer.Should().Be(customer);
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Success);
    }
    
    [Fact]
    public void OutputEnvelop_Should_ExecutedWithInput_TrowingException()
    {
        // Arrange
        var customer = new { 
            Id = 1
        };
        var processedCustomer = (object?)null;
        var exceptionToTrow = new Exception();

        // Act
        var outputEnvelop = OutputEnvelop.Execute(
            input: customer,
            handler: input =>
            {
                throw exceptionToTrow;
#pragma warning disable CS0162 // Unreachable code detected
                processedCustomer = input;
                return OutputEnvelop.CreateSuccess();
#pragma warning restore CS0162 // Unreachable code detected
            }
        );

        // Assert
        processedCustomer.Should().BeNull();
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Error);
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(1);
        outputEnvelop.ExceptionCollection.ToArray()[0].Should().Be(exceptionToTrow);
    }

    [Fact]
    public async Task OutputEnvelop_Should_Successfull_ExecutedAsync()
    {
        // Arrange
        var id = Guid.Empty;
        var cancellationTokenSource = new CancellationTokenSource();
        var receivedCancellationToken = (CancellationToken?)null;

        // Act
        var outputEnvelop = await OutputEnvelop.ExecuteAsync(
            handler: cancellationToken =>
            {
                receivedCancellationToken = cancellationToken;

                id = Guid.NewGuid();
                return Task.FromResult(OutputEnvelop.CreateSuccess());

            },
            cancellationToken: cancellationTokenSource.Token
        );

        // Assert
        id.Should().NotBe(default(Guid));
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Success);
        receivedCancellationToken.Should().Be(cancellationTokenSource.Token);
    }

    [Fact]
    public async Task OutputEnvelop_Should_ExecutedAsync_TrowingException()
    {
        // Arrange
        var id = Guid.Empty;
        var exceptionToTrow = new Exception();
        var cancellationTokenSource = new CancellationTokenSource();
        var receivedCancellationToken = (CancellationToken?)null;

        // Act
        var outputEnvelop = await OutputEnvelop.ExecuteAsync(
            handler: cancellationToken =>
            {
                receivedCancellationToken = cancellationToken;

                throw exceptionToTrow;
#pragma warning disable CS0162 // Unreachable code detected
                id = Guid.NewGuid();
                return Task.FromResult(OutputEnvelop.CreateSuccess());
#pragma warning restore CS0162 // Unreachable code detected
            },
            cancellationToken: cancellationTokenSource.Token
        );

        // Assert
        id.Should().Be(default(Guid));
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Error);
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(1);
        outputEnvelop.ExceptionCollection.ToArray()[0].Should().Be(exceptionToTrow);
        receivedCancellationToken.Should().Be(cancellationTokenSource.Token);
    }

    [Fact]
    public async Task OutputEnvelop_Should_Successfull_ExecutedWithInputAsync()
    {
        // Arrange
        var customer = new
        {
            Id = 1
        };
        var processedCustomer = (object?)null;
        var cancellationTokenSource = new CancellationTokenSource();
        var receivedCancellationToken = (CancellationToken?)null;

        // Act
        var outputEnvelop = await OutputEnvelop.ExecuteAsync(
        input: customer,
            handler: (input, cancellationToken) =>
            {
                receivedCancellationToken = cancellationToken;
                processedCustomer = input;

                return Task.FromResult(OutputEnvelop.CreateSuccess());
            },
            cancellationToken: cancellationTokenSource.Token
        );

        // Assert
        processedCustomer.Should().Be(customer);
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Success);
        receivedCancellationToken.Should().Be(cancellationTokenSource.Token);
    }

    [Fact]
    public async Task OutputEnvelop_Should_ExecutedWithInputAsync_TrowingException()
    {
        // Arrange
        var customer = new
        {
            Id = 1
        };
        var processedCustomer = (object?)null;
        var exceptionToTrow = new Exception();
        var cancellationTokenSource = new CancellationTokenSource();
        var receivedCancellationToken = (CancellationToken?)null;

        // Act
        var outputEnvelop = await OutputEnvelop.ExecuteAsync(
            input: customer,
            handler: (input, cancellationToken) =>
            {
                receivedCancellationToken = cancellationToken;
                throw exceptionToTrow;
#pragma warning disable CS0162 // Unreachable code detected
                processedCustomer = input;
                return Task.FromResult(OutputEnvelop.CreateSuccess());
#pragma warning restore CS0162 // Unreachable code detected
            },
            cancellationToken: cancellationTokenSource.Token
        );

        // Assert
        processedCustomer.Should().BeNull();
        outputEnvelop.Type.Should().Be(OutputEnvelopType.Error);
        outputEnvelop.ExceptionCollection.Span.Length.Should().Be(1);
        outputEnvelop.ExceptionCollection.ToArray()[0].Should().Be(exceptionToTrow);
        receivedCancellationToken.Should().Be(cancellationTokenSource.Token);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    public void OutputEnvelop_Should_Not_Created_With_Invalid_OutputEnvelopType(int outputEnvelopTypeInt)
    {
        // Arrange
        var outputEnvelopType = (OutputEnvelopType)outputEnvelopTypeInt;

        // Act
        var actHandler = () =>
        {
            OutputEnvelop.Create(outputEnvelopType);
        };

        // Assert
        actHandler.Should().Throw<InvalidOutputEnvelopTypeException>().Which.OutputEnvelopType.Should().Be(outputEnvelopType);
    }
}
