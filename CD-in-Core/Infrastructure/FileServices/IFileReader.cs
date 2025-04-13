namespace CD_in_Core.Infrastructure.FileServices
{
    public interface IFileReader
    {
        IAsyncEnumerable<List<byte>> ReadDigitsInBlocksAsync(string filePath, int blockSize, CancellationToken cancellationToken = default);
    }
}
