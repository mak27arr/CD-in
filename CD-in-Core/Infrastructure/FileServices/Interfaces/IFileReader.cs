using CD_in_Core.Infrastructure.FileServices.Reader;

namespace CD_in_Core.Infrastructure.FileServices.Interfaces
{
    public interface IFileReader
    {
        IAsyncEnumerable<PoolArray<byte>> ReadDigitsInBlocksAsync(string filePath, int blockSize, CancellationToken cancellationToken = default);
    }
}
