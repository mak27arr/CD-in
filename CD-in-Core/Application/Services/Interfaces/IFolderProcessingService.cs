namespace CD_in_Core.Application.Services.Interfaces
{
    public interface IFolderProcessingService
    {
        Task ProcessFolderAsync(string? folderPath, int blockSize, Action<double> progressCallback, CancellationToken cancellationToken);
    }
}