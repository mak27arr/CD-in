using CD_in_Core.Domain.Models;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface ILargeNumberExtractionService
    {
        Sequence ExtractLargeNumbers(Sequence sequence, LargeNumberExtractionOptions options);
    }
}
