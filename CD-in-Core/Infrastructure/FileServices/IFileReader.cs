namespace CD_in_Core.Infrastructure.FileServices
{
    public interface IFileReader
    {
        IAsyncEnumerable<List<int>> ReadDigitsInBlocksAsync(string filePath, int blockSize, CancellationToken cancellationToken = default);
    }
}
