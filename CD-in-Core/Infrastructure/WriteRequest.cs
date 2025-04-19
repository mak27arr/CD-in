using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Infrastructure.FileServices.Interfaces;

namespace CD_in_Core.Infrastructure
{
    public class WriteRequest
    {
        public required ISequence Sequence { get; init; }

        public required string SourceName { get; init; }

        public required ISequenceSaveParam SaveTo { get; init; }

        public Action<ISequence> OnWriteComplete { get; init; } 
    }
}
