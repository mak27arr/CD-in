using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services
{
    internal class FolderProcessingService : IFolderProcessingService
    {
        private readonly IMainProcessingService _mainProcessingService;

        public FolderProcessingService(IMainProcessingService mainProcessingService)
        {
            _mainProcessingService = mainProcessingService;
        }

        public async Task ProcessFolderAsync(ProcessingOption option, Action<double> progressCallback, CancellationToken cancellationToken)
        {
            await _mainProcessingService.ProcessFiles(option, progressCallback, cancellationToken);
        }
    }
}
