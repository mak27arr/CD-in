using CD_in_Core.Application.Settings;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces.Sequences
{
    internal interface ISequenceProcessingService
    {
        Task ProccesInputSequence(ProcessingOption option, IOutputDispatcherService sequenceWriter, string inputName, ISequence sequence, CancellationToken token);
    }
}