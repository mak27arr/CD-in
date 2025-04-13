using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.DeltaIndex;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices;
using System.Runtime.CompilerServices;

namespace CD_in_Core.Application.Services.DeltaIndex
{
    public class DeltaIndexProcessorService : IDeltaIndexProcessorService
    {
        private readonly IDeltaIndexService _deltaIndexService;
        private readonly IFileReader _fileReader;

        public DeltaIndexProcessorService(IDeltaIndexService deltaIndexService, IFileReader fileReader)
        {
            _deltaIndexService = deltaIndexService;
            _fileReader = fileReader;
        }

        public async IAsyncEnumerable<Element> ProcessFile(string filePath, DeltaIndexParams parameters, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            int globalOffset = 0;

            await using var enumerator = _fileReader.ReadDigitsInBlocksAsync(filePath, parameters.BlockSize, cancellationToken).GetAsyncEnumerator(cancellationToken);

            // Зчитуємо перший блок
            if (!await enumerator.MoveNextAsync())
                yield break;

            var currentBlock = enumerator.Current;
            var nextBlockTask = enumerator.MoveNextAsync().AsTask(); // Читаємо наступний блок у фоні

            while (true)
            {
                // Обробляємо поточний блок
                var delta = _deltaIndexService.ProcessBlock(currentBlock, globalOffset);

                foreach (var element in delta)
                {
                    yield return element;
                }

                globalOffset += currentBlock.Count;

                if (!await nextBlockTask)
                    yield break; // немає наступного блоку — завершити

                currentBlock = enumerator.Current;
                nextBlockTask = enumerator.MoveNextAsync().AsTask(); // Читаємо ще один блок у фоні
            }
        }
    }
}
