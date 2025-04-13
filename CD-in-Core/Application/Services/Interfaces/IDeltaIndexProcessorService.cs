using CD_in_Core.Domain.Models.DeltaIndex;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface IDeltaIndexProcessorService
    {
        IAsyncEnumerable<Element> ProcessFile(string filePath, DeltaIndexParams parameters, CancellationToken cancellationToken = default);
    }
}
