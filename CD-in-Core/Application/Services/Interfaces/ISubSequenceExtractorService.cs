using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface ISubSequenceExtractorService
    {
        Sequence ExstractSequence(Sequence sequence, SubSequenceExtractionOptions options);
    }
}
