using CD_in_Core.Extension;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using System.Runtime.CompilerServices;

namespace CD_in_Core.Infrastructure.FileServices.Reader
{
    public class FileReader : IFileReader
    {
        private const char zerroChar = '0';
        private readonly int _fileBufferSize;
        private readonly ILogger<FileReader> _logger;

        public FileReader(IConfiguration configuration, ILogger<FileReader> logger)
        {
            _logger = logger;
            _fileBufferSize = configuration.GetValue<int>("SequenceReaderSettings:FileBufferSize", 8192);
        }

        public async IAsyncEnumerable<PoolArray<byte>> ReadDigitsInBlocksAsync(
            string filePath,
            int blockSize,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var blockCurrentindex = 0;
            var block = new byte[blockSize];
            var fileReadBuffer = new char[_fileBufferSize];
            var arrayPool = CreateArrayPool(blockSize);

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: _fileBufferSize, useAsync: true);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var count = await reader.ReadAsync(fileReadBuffer, 0, fileReadBuffer.Length);

                for (int i = 0; i < count; i++)
                {
                    char namberChar = fileReadBuffer[i];
                    if (namberChar != '\r' && namberChar != '\n')
                    {
                        blockCurrentindex = AddToBlockIfValid(block, blockCurrentindex, namberChar);

                        if (blockCurrentindex >= blockSize)
                        {
                            yield return CloneToArrayFromPool(arrayPool, block, blockCurrentindex);
                            blockCurrentindex = 0;
                        }
                    }
                }
            }

            if (blockCurrentindex > 0)
                yield return CloneToArrayFromPool(arrayPool, block, blockCurrentindex);
        }

        private int AddToBlockIfValid(byte[] block, int index, char namberChar)
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

        private ObjectPool<byte[]> CreateArrayPool(int blockSize)
        {
            var poolProvider = new DefaultObjectPoolProvider();
            return poolProvider.Create(new ArrayPooledObjectPolicy<byte>(blockSize));
        }

        private static PoolArray<byte> CloneToArrayFromPool(ObjectPool<byte[]> pool, byte[] block, int index)
        {
            var result = new PoolArray<byte>(pool);
            result.Copy(block, index);
            return result;
        }
    }
}
