namespace CD_in_Core.Domain.Models.Sequences
{
    public class SequenceExtractionOptions : IOptions
    {
        public int TargetDigit { get; set; }

        public int MinSequenceLength { get; set; }
    }
}
