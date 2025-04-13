using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Infrastructure.FileServices
{
    public interface ISequenceWriter
    {
        Task AppendSequenceAsync(Sequence sequence, string sourceFileName, SequenceSaveOptions options, CancellationToken cancellationToken = default);
    }
}
