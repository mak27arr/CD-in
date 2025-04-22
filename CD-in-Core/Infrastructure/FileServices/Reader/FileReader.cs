using CD_in_Core.Extension;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
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
            var blockSize = fileSourceParam.BlockSize;
            var blockCurrentindex = 0;
            var fileReadBuffer = new byte[_fileBufferSize];
            var arrayPool = _singlePoolManager.GetOrCreateArrayPool(blockSize);
            var block = new PoolArray<byte>(arrayPool);
            var count = 0;
            using var stream = new FileStream(fileSourceParam.Path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: _fileBufferSize, useAsync: true);

            while ((count = await stream.ReadAsync(fileReadBuffer, 0, fileReadBuffer.Length, cancellationToken)) > 0 
                && !cancellationToken.IsCancellationRequested)
            {
                for (int i = 0; i < count; i++)
                {
                    var numberChar = fileReadBuffer[i];
                    if (numberChar == 0x0D || numberChar == 0x0A)
                        continue;

                    blockCurrentindex = AddToBlockIfValid(block, blockCurrentindex, numberChar);

                    if (blockCurrentindex == blockSize)
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

        private int AddToBlockIfValid(PoolArray<byte> destination, int index, byte numberChar)
        {
            byte number = (byte)(numberChar - zerroChar);
            if (number >= 0 && number < 2)
            {
                destination[index] = number;
                index++;
            }
            else
            {
                _logger.LogError("File contains illegal char: {0}", numberChar);
                throw new ArgumentException($"File contains illegal char: {numberChar}");
            }

            return index;
        }
    }
}
