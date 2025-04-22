using CD_in_Core.Application.Settings.DeltaIndex;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Reader;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IDeltaIndexTextFileReader
    {
        IAsyncEnumerable<IElement> ProcessFile(TextFileSourceParam fileSourceParam, Action<double> progressCallback, CancellationToken cancellationToken = default);
    }
}
