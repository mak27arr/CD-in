using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Models
{
    public class ExtractionOptions
    {
        public int ExecutionOrder { get; init; }

        public required IOptions SelectOption { get; set; }

        public required SequenceSaveOptions SaveOptions {get; set;}
    }
}