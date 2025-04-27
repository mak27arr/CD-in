using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;
using CD_in_Core.Domain.ValueObjects;

namespace CD_in_Core.Application.Services.Interfaces.Sequences
{
    internal interface ISequenceExtractionService
    {
        ISequence CloneSequence(ISequence sequence, RawSequenceExtractionRules extractionRules);
        ISequence Extract(ISequence sequence, SelectNumberRule options);
    }
}
