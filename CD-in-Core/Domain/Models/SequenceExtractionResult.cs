namespace CD_in_Core.Domain.Models
{
    public class SequenceExtractionResult
    {
        public int Digit { get; set; }
        public List<SequenceInfo> Sequences { get; set; } = new();
    }
}
