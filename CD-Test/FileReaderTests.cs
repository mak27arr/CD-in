using CD_in_Core.Extension;
using CD_in_Core.Infrastructure.FileServices.Reader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CD_Test
{
    public class FileReaderTests
    {
        private readonly Mock<ILogger<FileReader>> _loggerMock;
        private readonly IConfiguration _configuration;

        public FileReaderTests()
        {
            _loggerMock = new Mock<ILogger<FileReader>>();
            var inMemorySettings = new Dictionary<string, string?>
            {
            { "SequenceReaderSettings:FileBufferSize", "1000" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
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
            var reader = new FileReader(_configuration, _loggerMock.Object);
            var resultBlocks = new List<PoolArray<byte>>();
            var readParam = new TextFileSourceParam()
            {
                Path = filePath,
                BlockSize = blockSize
            };
            // Act
            await foreach (var block in reader.ReadDigitsInBlocksAsync(readParam))
            {
                resultBlocks.Add(block);
            }

            // Assert
            Assert.Equal(3, resultBlocks.Count);
            Assert.Equal(new byte[] { 0, 1, 0 }, resultBlocks[0].Data?.Take(resultBlocks[0].LastElementIndex));
            Assert.Equal(new byte[] { 1, 0, 1 }, resultBlocks[1].Data?.Take(resultBlocks[1].LastElementIndex));
            Assert.Equal(new byte[] { 0 }, resultBlocks[2].Data?.Take(resultBlocks[2].LastElementIndex));
        }

        [Fact]
        public async Task ReadDigitsInBlocksAsync_ThrowsOnInvalidCharInLine()
        {
            // Arrange
            var lines = new[] { "0", "1", "x", "1" };
            var filePath = CreateTempFile(lines);
            var reader = new FileReader(_configuration, _loggerMock.Object);
            var readParam = new TextFileSourceParam()
            {
                Path = filePath,
                BlockSize = 2
            };
            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await foreach (var _ in reader.ReadDigitsInBlocksAsync(readParam)) { }
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
