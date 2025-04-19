using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Interfaces.Sequences
{
    internal interface ISequenceExtractionService
    {
        ISequence CloneSequence(ISequence sequence);
        ISequence Extract(ISequence sequence, SelectNumberRule options);
    }
}
