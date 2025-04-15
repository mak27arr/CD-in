using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Channels;

namespace CD_in_Core.Infrastructure.FileServices.Writer
{
    public class SequenceWriter : ISequenceWriter
    {
        private readonly Channel<WriteRequest> _channel;
        private readonly Task _writeTask;
        private readonly CancellationTokenSource _cts = new();
        private readonly ILogger<SequenceWriter> _logger;
        private readonly int _maxSequenceInMemory;
        private readonly int _stringBufferSize;

        public SequenceWriter(IConfiguration configuration, ILogger<SequenceWriter> logger)
        {
            _logger = logger;
            _maxSequenceInMemory = configuration.GetValue<int>("SequenceWriterSettings:MaxSequenceInMemory", 3);
            _stringBufferSize = configuration.GetValue<int>("SequenceWriterSettings:DiskBufferSize", 4096);

            _channel = Channel.CreateBounded<WriteRequest>(new BoundedChannelOptions(_maxSequenceInMemory)
            {
                FullMode = BoundedChannelFullMode.Wait
            });

            _writeTask = Task.Run(async () => await ConsumeAsync(_cts.Token));
        }

        public async Task AppendSequenceAsync(WriteRequest writeRequest, CancellationToken token)
        {
            await _channel.Writer.WriteAsync(writeRequest, token);
        }

        public async Task WaitToFinishAsync()
        {
            _channel.Writer.TryComplete();
            await Task.WhenAny(Task.WhenAll(_writeTask, _channel.Reader.Completion), Task.Delay(TimeSpan.FromMinutes(3)));
        }

        private async Task ConsumeAsync(CancellationToken token)
        {
            await foreach (var request in _channel.Reader.ReadAllAsync(token))
            {
                try
                {
                    await AppendSequenceAsync(request.Sequence, request.SourceFileName, request.Options, token);
                    request.OnWriteComplete?.Invoke(request.Sequence);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving file: {0}", request?.Options?.FileName);
                }
            }
        }

        private async Task AppendSequenceAsync(ISequence sequence, string sourceFileName, SequenceSaveOptions options, CancellationToken cancellationToken = default)
        {
            string fullName = GetDestFilePath(sourceFileName, options);

            await using var stream = new FileStream(fullName, FileMode.Append, FileAccess.Write, FileShare.None, _stringBufferSize * 2, useAsync: true);
            await using var writer = new StreamWriter(stream);

            var contentBuffer = new StringBuilder(capacity: _stringBufferSize);
            int batchSize = _stringBufferSize / 2;
            int counter = 0;

            foreach (var digit in sequence)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                contentBuffer.Append(digit.Key).Append(':').Append(digit.Value).AppendLine();
                counter++;

                if (counter >= batchSize)
                {
                    await writer.WriteAsync(contentBuffer);
                    contentBuffer.Clear();
                    counter = 0;
                }
            }

            if (contentBuffer.Length > 0)
            {
                await writer.WriteAsync(contentBuffer);
            }
        }

        private static string GetDestFilePath(string sourceFileName, SequenceSaveOptions options)
        {
            var fileName = $"{options.FileName}-{sourceFileName}.txt";
            var fullName = Path.Combine(options.FilePath, fileName);
            return fullName;
        }
    }
}
