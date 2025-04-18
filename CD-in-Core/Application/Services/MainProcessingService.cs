using CD_in_Core.Application.Settings;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Application.Settings.Input;

namespace CD_in_Core.Application.Services
{
    internal class MainProcessingService : IMainProcessingService
    {
        private readonly IDirectoryProcessingService _directoryProcessingService;

        public MainProcessingService(IDirectoryProcessingService directoryProcessingService)
        {
            _directoryProcessingService = directoryProcessingService;
        }

        public async Task ProcessAsync(ProcessingOption option, Action<double> progressCallback, CancellationToken cancellationToken)
        {
            switch (option.InputSource)
            {
                case DirectoryInputSourceSettings:
                    await _directoryProcessingService.ProcessDirectory(option, progressCallback, cancellationToken);
                    break;
                default:
                    throw new NotImplementedException($"Can`t process input source {option?.InputSource.GetType().Name}");
            }
        }
    }
}
