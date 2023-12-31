﻿using MCIO.OutputEnvelop.Utils;

namespace MCIO.OutputEnvelop.UnitTests.UtilsTests;
public class ArrayUtilsTest
{
    [Fact]
    public void ArrayUtils_Should_CopyToExistingArray_FromExistingArray()
    {
        // Arrange
        var sourceArray = new int[] { 1, 2, 3 };
        var destinationArray = new int[sourceArray.Length];
        var destinationIndex = 0;

        // Act
        ArrayUtils.CopyToExistingArray(
            destinationArray,
            destinationIndex,
            sourceArray
        );

        // Assert
        destinationArray.Should().BeEquivalentTo(sourceArray);
    }

    [Fact]
    public void ArrayUtils_Should_CopyToExistingArray_FromExistingEnumerable()
    {
        // Arrange
        var sourceArray = Enumerable.Range(start: 1, count: 3);
        var destinationArray = new int[sourceArray.Count()];
        var destinationIndex = 0;

        // Act
        ArrayUtils.CopyToExistingArray(
            destinationArray,
            destinationIndex,
            sourceArray
        );

        // Assert
        destinationArray.Should().BeEquivalentTo(sourceArray);
    }

    [Fact]
    public void ArrayUtils_Should_AddNewItem()
    {
        // Arrange
        var sourceArray = new int[] { 1, 2, 3 };
        var newItem = 4;

        // Act
        var newArray = ArrayUtils.AddNewItem(
            sourceArray,
            newItem
        );

        // Assert
        sourceArray.Should().BeSubsetOf(newArray);
        newArray.Should().HaveCount(sourceArray.Length + 1);
        newArray.Last().Should().Be(newItem);
    }

    [Fact]
    public void ArrayUtils_Should_Not_AddNewItem()
    {
        // Arrange
        var sourceArray = (string[]?)null;
        var newItem = (string?)null;

        // Act
        var newArray = ArrayUtils.AddNewItem(
            sourceArray,
            newItem
        );

        // Assert
        newArray.Should().BeNull();
    }

    [Fact]
    public void ArrayUtils_Should_AddNewItem_From_NullableSourceArray()
    {
        // Arrange
        var sourceArray = (string[]?)null;
        var newItem = "A";

        // Act
        var newArray = ArrayUtils.AddNewItem(
            sourceArray,
            newItem
        );

        // Assert
        newArray.Should().HaveCount(1);
        newArray.Last().Should().Be(newItem);
    }

    [Fact]
    public void ArrayUtils_Should_Not_AddNewItem_WithNullNewItem()
    {
        // Arrange
        var sourceArray = new string[] { "A", "B", "C" };
        var newItem = (string?)null;

        // Act
        var newArray = ArrayUtils.AddNewItem(
            sourceArray,
            newItem
        );

        // Assert
        sourceArray.Should().BeEquivalentTo(newArray);
    }

    [Fact]
    public void ArrayUtils_Should_AddRange()
    {
        // Arrange
        var sourceArrayA = new int[] { 1, 2, 3 };
        var sourceArrayB = new int[] { 1, 2, 3 };
        var newItemA = new int[]{ 4, 5 };
        var newItemC = new int[] { 4, 5 };

        // Act
        var newArrayA = ArrayUtils.AddRange(sourceArrayA, newItemA);
        var newArrayB = ArrayUtils.AddRange(sourceArrayB, null);
        var newArrayC = ArrayUtils.AddRange(null, newItemC);
        var newArrayD = ArrayUtils.AddRange<int>(null, null);

        // Assert
        sourceArrayA.Should().BeSubsetOf(newArrayA);
        newItemA.Should().BeSubsetOf(newArrayA);
        newArrayA.Should().HaveCount(sourceArrayA.Length + newItemA.Length);

        sourceArrayB.Should().BeSubsetOf(newArrayB);
        newArrayB.Length.Should().Be(sourceArrayB.Length);

        newItemC.Should().BeSubsetOf(newArrayC);
        newArrayC.Length.Should().Be(newItemC.Length);

        newArrayD.Should().BeNull();
    }

    [Fact]
    public void ArrayUtils_Should_Not_AddRange()
    {
        // Arrange
        var sourceArray = (string[]?)null;
        var newItem = (string[]?)null;

        // Act
        var newArray = ArrayUtils.AddRange(
            sourceArray,
            newItem
        );

        // Assert
        newArray.Should().BeNull();
    }

    [Fact]
    public void ArrayUtils_Should_AddRange_From_NullableSourceArray()
    {
        // Arrange
        var sourceArray = (string[]?)null;
        var newItem = new string[] { "A", "B" };

        // Act
        var newArray = ArrayUtils.AddRange(
            sourceArray,
            newItem
        );

        // Assert
        newArray.Should().HaveCount(newItem.Length);
        newItem.Should().BeSubsetOf(newArray);
    }

    [Fact]
    public void ArrayUtils_Should_Not_AddRange_WithNullNewItem()
    {
        // Arrange
        var sourceArray = new string[] { "A", "B", "C" };
        var newItem = (string[]?)null;

        // Act
        var newArray = ArrayUtils.AddRange(
            sourceArray,
            newItem
        );

        // Assert
        sourceArray.Should().BeEquivalentTo(newArray);
    }
}
