using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services.Interfaces
{
    public interface IDeltaIndexProcessorService
    {
        Task<DeltaIndexResult> ProcessFile(string filePath, DeltaIndexParams parameters, CancellationToken cancellationToken = default);
    }
}
