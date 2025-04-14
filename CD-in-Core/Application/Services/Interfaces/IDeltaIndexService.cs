using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IDeltaIndexService
    {
        IEnumerable<Element> ProcessBlock(IEnumerable<byte> digits, int globalOffset);
    }
}
