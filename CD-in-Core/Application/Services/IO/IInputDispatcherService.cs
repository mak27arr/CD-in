using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Interfaces;

namespace CD_in_Core.Application.Services.IO
{
    internal interface IInputDispatcherService
    {
        IAsyncEnumerable<IElement> GetInputDelta(IInputSourceParam inputSourceParam, Action<double> progress, CancellationToken token = default);
    }
}