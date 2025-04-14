using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Writer;

namespace CD_in_Core.Infrastructure.FileServices.Interfaces
{
    public interface ISequenceWriter
    {
        Task AppendSequenceAsync(WriteRequest writeRequest, CancellationToken token);

        Task WaitToFinishAsync();
    }
}
