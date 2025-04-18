using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface ILargeNumberExtractionService
    {
        ISequence ExtractLargeNumbers(ISequence sequence, LargeNumberExtraction options);
    }
}
