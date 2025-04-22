using CD_in_Core.Application.Settings;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface IMainProcessingService
    {
        Task ProcessAsync(ProcessingOption option, Action<double> progressCallback, CancellationToken cancellationToken);
    }
}