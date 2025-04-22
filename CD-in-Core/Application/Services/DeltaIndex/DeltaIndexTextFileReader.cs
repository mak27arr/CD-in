using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Interfaces;
using CD_in_Core.Infrastructure.FileServices.Reader;
using System.Runtime.CompilerServices;

namespace CD_in_Core.Application.Services.DeltaIndex
{
    internal class DeltaIndexTextFileReader : IDeltaIndexTextFileReader
    {
        private readonly IDeltaIndexService _deltaIndexService;
        private readonly IFileReader _fileReader;
        private IFileReadProgressTracker _fileReadProgressTracker;


        public DeltaIndexTextFileReader(IDeltaIndexService deltaIndexService, 
            IFileReader fileReader,
            IFileReadProgressTracker fileReadProgressTracker)
        {
            _deltaIndexService = deltaIndexService;
            _fileReader = fileReader;
            _fileReadProgressTracker = fileReadProgressTracker;
        }

        public async IAsyncEnumerable<KeyValuePair<int, int>> ProcessFile(string filePath, 
            DeltaIndexParams parameters,
            Action<double> progressCallback,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            int globalOffset = 0;

            _fileReadProgressTracker.Initialize(fileSourceParam.Path);
            await using var enumerator = _fileReader.ReadDigitsInBlocksAsync(fileSourceParam, cancellationToken).GetAsyncEnumerator(cancellationToken);

            if (!await enumerator.MoveNextAsync())
                yield break;

            var currentBlock = enumerator.Current;
            var nextBlockTask = enumerator.MoveNextAsync().AsTask();

            while (true)
            {
                var elementsDelta = _deltaIndexService.ProcessBlock(currentBlock);
                globalOffset += currentBlock.LastElementIndex;
                currentBlock.Release();

                foreach (var element in elementsDelta)
                {
                    yield return element;
                }

                if (elementsDelta is IPooledSequence pooledSequence)
                    pooledSequence.Release();

                var progress = _fileReadProgressTracker.GetProgressPercentage(globalOffset);
                progressCallback(progress);


                if (!await nextBlockTask.ConfigureAwait(false))
                    yield break;

                currentBlock = enumerator.Current;
                nextBlockTask = enumerator.MoveNextAsync().AsTask();
            }
        }
    }
}
