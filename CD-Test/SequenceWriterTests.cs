namespace CD_Test;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;
using CD_in_Core.Infrastructure.FileServices.Writer;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Application.Settings;
using CD_in_Core.Infrastructure;
using CD_in_Core.Application;

public class SequenceWriterTests
{
    private readonly Mock<ILogger<SequenceWriter>> _mockLogger;
    private readonly SequenceWriter _sequenceWriter;
    private readonly string _tempDirectory;

    public SequenceWriterTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "SequenceWriterTests");
        Directory.CreateDirectory(_tempDirectory);

        _mockLogger = new Mock<ILogger<SequenceWriter>>();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "SequenceWriterSettings:MaxSequenceInMemory", "3" } // Ти можеш додавати інші ключі за потреби
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _sequenceWriter = new SequenceWriter(configuration, _mockLogger.Object);
    }

    [Fact]
    public async Task AppendSequenceAsync_ShouldProcessFirstTwoRequestsWithoutDelay()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var sequence = new Sequence(2);
        sequence.Add(new Element(1, 1, 10));
        sequence.Add(new Element(2, 2, 20));
        var request = new WriteRequest
        {
            Sequence = sequence,
            SourceName = "file1.txt",
            SaveTo = new SaveToTextFileParam { FilePath = "C:\\temp", FileName = "output" }
        };

        // Act
        var startTime = DateTime.Now;
        await _sequenceWriter.AppendSequenceAsync(request, cancellationToken);
        await _sequenceWriter.AppendSequenceAsync(request, cancellationToken);
        var endTimeAfterSecondRequest = DateTime.Now;

        var firstRequestDuration = endTimeAfterSecondRequest - startTime;

        // Assert
        Assert.True(firstRequestDuration < TimeSpan.FromMilliseconds(200), "Request took too long.");
    }

    [Fact]
    public async Task AppendSequenceAsync_ShouldCreateFileAndWriteCorrectContent()
    {
        // Arrange
        var cancellationToken = new CancellationToken();

        // Create a WriteRequest with some sequence data
        var sequence = new Sequence(2);
        sequence.Add(new Element(1, 1, 0));
        sequence.Add(new Element(2, 2, 1));
        var request = new WriteRequest
        {
            Sequence = sequence,
            SourceName = "file1",
            SaveTo = new SaveToTextFileParam
            {
                FilePath = _tempDirectory,
                FileName = "output"
            }
        };

        // Act
        await _sequenceWriter.AppendSequenceAsync(request, cancellationToken);
        await _sequenceWriter.WaitToFinishAsync();

        var filePath = Path.Combine(_tempDirectory, "output-file1.txt");

        Assert.True(File.Exists(filePath), $"File {filePath} was not created.");

        var fileContent = await File.ReadAllTextAsync(filePath);
        var expectedContent = "1:0\r\n2:1\r\n";

        File.Delete(filePath);
        Assert.Equal(expectedContent, fileContent);
    }
}

