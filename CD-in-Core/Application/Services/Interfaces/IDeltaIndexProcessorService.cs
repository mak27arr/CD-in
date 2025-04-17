using CD_in_Core.Domain.Models.DeltaIndex;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IDeltaIndexProcessorService
    {
        IAsyncEnumerable<IElement> ProcessFile(string filePath, DeltaIndexParams parameters, Action<double> progressCallback,CancellationToken cancellationToken = default);
    }
}
