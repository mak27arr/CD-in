﻿namespace CD_Test;
using Moq;
using Xunit;
using Microsoft.Extensions.ObjectPool;
using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.DeltaIndex;
using CD_in_Core.Extension;
using CD_in_Core.Domain.Models.Sequences;

public class DeltaIndexServiceTests
{
    private readonly Mock<ISequencePool> _sequencePoolMock;
    private readonly Mock<IPooledSequence> _sequenceMock;
    private readonly Mock<ObjectPool<byte[]>> _arrayPoolMock;
    private readonly DeltaIndexService _service;

    public DeltaIndexServiceTests()
    {
        _sequencePoolMock = new Mock<ISequencePool>();
        _sequenceMock = new Mock<IPooledSequence>();
        _sequencePoolMock.Setup(p => p.Get()).Returns(_sequenceMock.Object);

        _arrayPoolMock = new Mock<ObjectPool<byte[]>>();
        _arrayPoolMock.Setup(p => p.Get()).Returns(new byte[100]);

        _service = new DeltaIndexService(_sequencePoolMock.Object);
    }

    [Fact]
    public void ProcessBlock_EmptyArray_ReturnsNull()
    {
        // Arrange
        var poolArray = new PoolArray<byte>(_arrayPoolMock.Object);
        poolArray.Copy(new byte[0], 0);

        // Act
        var result = _service.ProcessBlock(poolArray);

        // Assert
        Assert.Equal(0, result.Count);
        poolArray.Release();
    }

    [Fact]
    public void ProcessBlock_FewerOnes_ReturnsSequenceWithOneIndexesAndDeltas()
    {
        // Arrange
        var input = new byte[] { 0, 1, 0, 1, 0 };
        var poolArray = new PoolArray<byte>(_arrayPoolMock.Object);
        poolArray.Copy(input, input.Length);
        _sequenceMock.Setup(s => s.Add(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<int, int, int>((index, displayIndex, delta) => { });

        // Act
        var result = _service.ProcessBlock(poolArray);

        // Assert
        _sequenceMock.Verify(s => s.Add(1, 1, 2), Times.Once());
        _sequenceMock.Verify(s => s.Add(3, 2,2), Times.Once());
        Assert.Equal(_sequenceMock.Object, result);
        poolArray.Release();
    }

    [Fact]
    public void ProcessBlock_FewerZeros_ReturnsSequenceWithZeroIndexesAndDeltas()
    {
        // Arrange
        var input = new byte[] { 1, 0, 1, 0, 1 };
        var poolArray = new PoolArray<byte>(_arrayPoolMock.Object);
        poolArray.Copy(input, input.Length);
        _sequenceMock.Setup(s => s.Add(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<int, int, int>((index, displayIndex, delta) => { });

        // Act
        var result = _service.ProcessBlock(poolArray);

        // Assert
        _sequenceMock.Verify(s => s.Add(1, 1, 2), Times.Once());
        _sequenceMock.Verify(s => s.Add(3, 2, 2), Times.Once());
        Assert.Equal(_sequenceMock.Object, result);
        poolArray.Release();
    }

    [Fact]
    public void ProcessBlock_EqualOnesAndZeros_ReturnsSequenceWithZeroIndexesAndDeltas()
    {
        // Arrange
        var input = new byte[] { 1, 0, 1, 0 };
        var poolArray = new PoolArray<byte>(_arrayPoolMock.Object);
        poolArray.Copy(input, input.Length);
        _sequenceMock.Setup(s => s.Add(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<int, int, int>((index, displayIndex, delta) => { });

        // Act
        var result = _service.ProcessBlock(poolArray);

        // Assert
        _sequenceMock.Verify(s => s.Add(1, 1, 2), Times.Once());
        _sequenceMock.Verify(s => s.Add(3, 2, 2), Times.Once());
        Assert.Equal(_sequenceMock.Object, result);
        poolArray.Release();
    }

    [Fact]
    public void CalculateDelta_EmptyList_ReturnsNull()
    {
        // Act
        var result = _service.CalculateDelta(new List<int>());

        // Assert
        Assert.Equal(0, result.Count);
    }

    [Fact]
    public void CalculateDelta_SingleIndex_ReturnsSequenceWithSingleIndexAndDelta()
    {
        // Arrange
        var indexes = new List<int> { 5 };
        _sequenceMock.Setup(s => s.Add(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<int, int, int>((index, displayIndex, delta) => { });

        // Act
        var result = _service.CalculateDelta(indexes);

        // Assert
        _sequenceMock.Verify(s => s.Add(5, 1, 6), Times.Once());
        Assert.Equal(_sequenceMock.Object, result);
    }

    [Fact]
    public void CalculateDelta_MultipleIndexes_ReturnsSequenceWithIndexesAndDeltas()
    {
        // Arrange
        var indexes = new List<int> { 1, 3, 6 };
        _sequenceMock.Setup(s => s.Add(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Callback<int, int, int>((index, displayIndex, delta) => { });

        // Act
        var result = _service.CalculateDelta(indexes);

        // Assert
        _sequenceMock.Verify(s => s.Add(1, 1, 2), Times.Once());
        _sequenceMock.Verify(s => s.Add(3, 2, 2), Times.Once());
        _sequenceMock.Verify(s => s.Add(6, 3, 3), Times.Once());
        Assert.Equal(_sequenceMock.Object, result);
    }

    [Fact]
    public void ProcessBlock_WithValidBinarySequence_ReturnsExpectedDeltaValues()
    {
        _sequencePoolMock.Setup(p => p.Get()).Returns(new PooledSequence(100, _sequencePoolMock.Object));

        // Arrange
        var inputData = ReadArrayFromFile<byte>("TestData/Delta/binary_input_1.txt");
        var poolArray = new PoolArray<byte>(_arrayPoolMock.Object);
        poolArray.Copy(inputData, inputData.Length);

        var expectedDelta = ReadArrayFromFile<int>("TestData/Delta/expected_delta_1.txt");

        // Act
        var result = _service.ProcessBlock(poolArray);
        var actualDelta = result.Select(e => e.Value).ToList();

        // Assert
        Assert.Equal(expectedDelta, actualDelta);
    }

    private static T[] ReadArrayFromFile<T>(string path) where T : IParsable<T>
    {
        return File.ReadAllLines(path)
                   .Select(line => T.Parse(line, null))
                   .ToArray();
    }
}