using CD_in_Core.Infrastructure.FileServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace CD_in_Core.Infrastructure.FileServices.Reader
{
    public class FileReader : IFileReader
    {
        private const char zerroChar = '0';
        private readonly ILogger<FileReader> _logger;

        public FileReader(ILogger<FileReader> logger)
        {
            _logger = logger;
        }

        public async IAsyncEnumerable<List<byte>> ReadDigitsInBlocksAsync(
            string filePath,
            int blockSize,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var channel = Channel.CreateBounded<char>(new BoundedChannelOptions(blockSize)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
            var block = new List<byte>(blockSize);

            var reedTask = Task.Run(ReadFileContentToChannel(filePath, channel, cancellationToken));

            await foreach (var lineChar in channel.Reader.ReadAllAsync(cancellationToken))
            {
                byte number = (byte)(lineChar - zerroChar);

                if (number >= 0 && number < 10)
                {
                    block.Add(number);
                }
                else
                {
                    _logger.LogError("File contains illegal char: {0} symbol: {1}", lineChar, lineChar.ToString());
                    throw new ArgumentException($"File contains illegal char: {lineChar}");
                }

                if (block.Count >= blockSize)
                {
                    yield return new List<byte>(block);
                    block.Clear();
                }
            }

            await reedTask;
            if (reedTask.Exception != null)
                throw reedTask.Exception.InnerException ?? reedTask.Exception;

            if (block.Count > 0)
                yield return block;
        }

        private Func<Task?> ReadFileContentToChannel(string filePath, Channel<char> channel, CancellationToken cancellationToken)
        {
            return async () =>
            {
                try
                {
                    var buffer = new char[1000];
                    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
                    using var reader = new StreamReader(stream);
                    while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
                    {
                        var count = await reader.ReadAsync(buffer, 0, buffer.Length);
                        var filteredChars = buffer.Take(count).Where(c => c != '\r' && c != '\n').ToArray();

                        foreach (var namberChar in filteredChars.Take(count))
                        {
                                await channel.Writer.WriteAsync(namberChar, cancellationToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error wile reading {0}", filePath);
                }
                finally
                {
                    channel.Writer.Complete();
                }
            };
        }
    }
}
