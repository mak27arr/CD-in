using CD_in_Core.Extension;
using CD_in_Core.Infrastructure.FileServices.Reader;

namespace CD_in_Core.Infrastructure.FileServices.Interfaces
{
    public interface IFileReader
    {
        IAsyncEnumerable<PoolArray<byte>> ReadDigitsInBlocksAsync(TextFileSourceParam fileSourceParam, CancellationToken cancellationToken = default);
    }
}
