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
            if(existingOutputEnvelop.OutputMessageCollection.Length > 0)
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
}
