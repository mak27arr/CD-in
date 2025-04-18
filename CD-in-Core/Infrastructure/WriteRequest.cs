using CD_in_Core.Application.Settings;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Infrastructure
{
    public class WriteRequest
    {
        public required ISequence Sequence { get; init; }

        public required string SourceFileName { get; init; }

        public required ISequenceSaveSettings SaveTo { get; init; }

        public Action<ISequence> OnWriteComplete { get; init; } 
    }
}
