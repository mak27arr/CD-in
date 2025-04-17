using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface IFolderProcessingService
    {
        Task ProcessFolderAsync(ProcessingOption option, Action<double> progressCallback, CancellationToken cancellationToken);
    }
}