using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface ISequenceExtractorService
    {
        void ProcessBlock(IEnumerable<int> digits, int globalOffset);
        SequenceExtractionResult GetResult();
        void Reset();
    }
}
