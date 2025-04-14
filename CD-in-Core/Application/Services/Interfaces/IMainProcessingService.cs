using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IMainProcessingService
    {
        Task ProcessFiles(ProcessingOption option, Action<double> progressCallback, CancellationToken token);
    }
}