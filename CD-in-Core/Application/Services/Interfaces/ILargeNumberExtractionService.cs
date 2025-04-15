using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface ILargeNumberExtractionService
    {
        ISequence ExtractLargeNumbers(ISequence sequence, LargeNumberExtractionOptions options);
    }
}
