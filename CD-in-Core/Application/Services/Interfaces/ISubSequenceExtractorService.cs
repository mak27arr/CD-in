using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface ISubSequenceExtractorService
    {
        ISequence ExstractSequence(ISequence sequence, SubSequenceExtractionOptions options);
    }
}
