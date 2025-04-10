﻿using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;

namespace CD_in_Core.Infrastructure.FileServices
{
    public class FileReader : IFileReader
    {
        public async IAsyncEnumerable<List<int>> ReadDigitsInBlocksAsync(
            string filePath,
            int blockSize,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            using var reader = new StreamReader(stream);

            var block = new List<int>(blockSize);

            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync(cancellationToken);

                if (string.IsNullOrWhiteSpace(line))
                    throw new ArgumentException("File contain empty string");

                if (int.TryParse(line, CultureInfo.InvariantCulture, out var number))
                    block.Add(number);
                else
                    throw new ArgumentException($"File contains illegal character: {line}");

                if (block.Count >= blockSize)
                {
                    yield return new List<int>(block);
                    block.Clear();
                }
            }

            if (block.Count > 0)
                yield return block;
        }
    }
}
