using CD_in_Core.Application.Settings.DeltaIndex;
using CD_in_Core.Infrastructure.FileServices.Reader;
using System.Runtime.CompilerServices;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IDeltaIndexTextFileReader
    {
        IAsyncEnumerable<KeyValuePair<int, int>> ProcessFile(TextFileSourceParam filePath,
            Action<double> progressCallback,
            [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
