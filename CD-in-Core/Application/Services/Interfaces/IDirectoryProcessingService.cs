using CD_in_Core.Application.Settings;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IDirectoryProcessingService
    {
        Task ProcessDirectory(ProcessingOption option, Action<double> progressCallback, CancellationToken token);
    }
}