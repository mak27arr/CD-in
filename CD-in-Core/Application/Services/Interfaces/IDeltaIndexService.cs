using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Extension;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IDeltaIndexService
    {
        IEnumerable<Element> ProcessBlock(PoolArray<byte> digits, int globalOffset);
    }
}
