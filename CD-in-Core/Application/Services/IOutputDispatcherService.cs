using CD_in_Core.Infrastructure;

namespace CD_in_Core.Application.Services
{
    internal interface IOutputDispatcherService
    {
        Task AppendSequenceAsync(WriteRequest writeRequest, CancellationToken token);
        Task WaitToFinishAsync();
    }
}