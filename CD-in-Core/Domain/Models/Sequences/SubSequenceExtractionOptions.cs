using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Sequences
{
    public class SubSequenceExtractionOptions : BaseOption
    {
        public IValueCondition<int> Condition { get; init; } = default!;

        public int MinSequenceLength { get; init; }
    }
}
