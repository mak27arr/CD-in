using CD_in_Core.Application.Settings;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces.Sequences
{
    internal interface ISequenceProcessingService
    {
        Task ProccesInputSequence(ISequence sequence, ProcessingOption option, IOutputDispatcherService outputDispatcher, string inputName, CancellationToken token);
    }
}