using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface IDeltaIndexService
    {
        void ProcessBlock(IEnumerable<int> digits, int globalOffset);
        DeltaIndexResult GetResult();
        void Reset();
    }
}
