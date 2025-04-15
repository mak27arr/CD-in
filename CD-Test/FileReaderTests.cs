﻿using CD_in_Core.Infrastructure.FileServices.Reader;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CD_Test
{
    public class FileReaderTests
    {
        private readonly Mock<ILogger<FileReader>> _loggerMock;

        public FileReaderTests()
        {
            _loggerMock = new Mock<ILogger<FileReader>>();
        }

        private string CreateTempFile(string[] lines)
        {
            var tempPath = Path.GetTempFileName();
            File.WriteAllLines(tempPath, lines);
            return tempPath;
        }

        [Fact]
        public async Task ReadDigitsInBlocksAsync_ReadsLinesOfZeroAndOne_Correctly()
        {
            // Arrange
            var lines = new[] { "0", "1", "0", "1", "0", "1", "0" };
            var blockSize = 3;
            var filePath = CreateTempFile(lines);
            var reader = new FileReader(_loggerMock.Object);
            var resultBlocks = new List<PoolArray<byte>>();

            // Act
            await foreach (var block in reader.ReadDigitsInBlocksAsync(filePath, blockSize))
            {
                resultBlocks.Add(block);
            }

            // Assert
            Assert.Equal(3, resultBlocks.Count);
            Assert.Equal(new byte[] { 0, 1, 0 }, resultBlocks[0].Data);
            Assert.Equal(new byte[] { 1, 0, 1 }, resultBlocks[1].Data);
            Assert.Equal(new byte[] { 0 }, resultBlocks[2].Data);
        }

        [Fact]
        public async Task ReadDigitsInBlocksAsync_ThrowsOnInvalidCharInLine()
        {
            // Arrange
            var lines = new[] { "0", "1", "x", "1" };
            var filePath = CreateTempFile(lines);
            var reader = new FileReader(_loggerMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await foreach (var _ in reader.ReadDigitsInBlocksAsync(filePath, blockSize: 2)) { }
            });

            Assert.Contains("illegal char", ex.Message);
            _loggerMock.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("illegal char")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
