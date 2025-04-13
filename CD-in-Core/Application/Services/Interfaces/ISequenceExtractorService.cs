using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface ISequenceExtractorService
    {
        Sequence ExstractSequence(Sequence sequence, SequenceExtractionOptions options);
    }
}
