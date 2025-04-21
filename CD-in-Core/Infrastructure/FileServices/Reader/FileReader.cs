using CD_in_Core.Extension;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace CD_in_Core.Infrastructure.FileServices.Reader
{
    public class FileReader : IFileReader
    {
        private const char zerroChar = '0';
        private readonly int _fileBufferSize;
        private readonly SinglePoolManager _singlePoolManager;
        private readonly ILogger<FileReader> _logger;

        public FileReader(SinglePoolManager singlePoolManager, IConfiguration configuration, ILogger<FileReader> logger)
        {
            _singlePoolManager = singlePoolManager;
            _logger = logger;
            _fileBufferSize = configuration.GetValue<int>("SequenceReaderSettings:FileBufferSize", 8192);
        }

        public async IAsyncEnumerable<PoolArray<byte>> ReadDigitsInBlocksAsync(TextFileSourceParam fileSourceParam,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var blockCurrentindex = 0;
            var fileReadBuffer = new char[_fileBufferSize];
            var arrayPool = _singlePoolManager.GetOrCreateArrayPool(fileSourceParam.BlockSize);
            var block = new PoolArray<byte>(arrayPool);
            using var stream = new FileStream(fileSourceParam.Path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: _fileBufferSize, useAsync: true);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var count = await reader.ReadAsync(fileReadBuffer, 0, fileReadBuffer.Length);

                for (int i = 0; i < count; i++)
                {
                    char numberChar = fileReadBuffer[i];
                    if (numberChar == '\r' || numberChar == '\n')
                        continue;

                    blockCurrentindex = AddToBlockIfValid(block, blockCurrentindex, numberChar);

                    if (blockCurrentindex == fileSourceParam.BlockSize)
                    {
                        yield return block;
                        blockCurrentindex = 0;
                        block = new PoolArray<byte>(arrayPool);
                    }
                }
            }

            if (blockCurrentindex > 0)
                yield return block;
        }

        private int AddToBlockIfValid(PoolArray<byte> block, int index, char namberChar)
            {
            byte number = (byte)(namberChar - zerroChar);
            if (number >= 0 && number < 10)
            {
                block[index] = number;
                index++;
            }
            else
            {
                _logger.LogError("File contains illegal char: {0} symbol: {1}", namberChar, namberChar.ToString());
                throw new ArgumentException($"File contains illegal char: {namberChar}");
            }

            return index;
        }
    }
}
