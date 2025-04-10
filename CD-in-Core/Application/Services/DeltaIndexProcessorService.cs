using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;
using CD_in_Core.Infrastructure.FileServices;

namespace CD_in_Core.Application.Services
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

        public async Task<DeltaIndexResult> ProcessFile(string filePath, DeltaIndexParams parameters, CancellationToken cancellationToken = default)
        {
            _deltaIndexService.Reset();

            int globalOffset = 0;

            await foreach (var block in _fileReader.ReadDigitsInBlocksAsync(filePath, parameters.BlockSize, cancellationToken))
            {
                _deltaIndexService.ProcessBlock(block, globalOffset);
                globalOffset += block.Count;
            }

            return _deltaIndexService.GetResult();
        }
    }
}
