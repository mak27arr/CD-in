using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Models
{
    public class ExtractionOptions
    {
        public IOptions SelectOption { get; set; }

        public SequenceSaveOptions SaveOptions {get; set;}
    }
}