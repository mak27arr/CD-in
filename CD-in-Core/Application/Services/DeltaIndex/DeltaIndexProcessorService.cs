﻿using CD_in_Core.Application.Services.Interfaces;
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
        private IFileReadProgressTracker _fileReadProgressTracker;


        public DeltaIndexProcessorService(IDeltaIndexService deltaIndexService, 
            IFileReader fileReader,
            IFileReadProgressTracker fileReadProgressTracker)
        {
            _deltaIndexService = deltaIndexService;
            _fileReader = fileReader;
            _fileReadProgressTracker = fileReadProgressTracker;
        }

        public async IAsyncEnumerable<Element> ProcessFile(string filePath, 
            DeltaIndexParams parameters,
            Action<double> progressCallback,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            int globalOffset = 0;

            _fileReadProgressTracker.Initialize(filePath);
            await using var enumerator = _fileReader.ReadDigitsInBlocksAsync(filePath, parameters.BlockSize, cancellationToken).GetAsyncEnumerator(cancellationToken);

            if (!await enumerator.MoveNextAsync())
                yield break;

            var currentBlock = enumerator.Current;
            var nextBlockTask = enumerator.MoveNextAsync().AsTask();

            while (true)
            {
                var delta = _deltaIndexService.ProcessBlock(currentBlock, globalOffset);

                foreach (var element in delta)
                {
                    yield return element;
                }

                globalOffset += currentBlock.Count;

                if (progressCallback != null)
                {
                    var progress = _fileReadProgressTracker.GetProgressPercentage(globalOffset);
                    progressCallback.Invoke(progress);
                }

                if (!await nextBlockTask)
                    yield break;

                currentBlock = enumerator.Current;
                nextBlockTask = enumerator.MoveNextAsync().AsTask();
            }
        }
    }
}
