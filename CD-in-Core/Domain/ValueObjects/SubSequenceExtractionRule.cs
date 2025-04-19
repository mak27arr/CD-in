using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class SubSequenceExtractionRule : BaseExtractionRule
    {
        public required IValueCondition<IElement> Condition { get; init; }

        public required int MinSequenceLength { get; init; }
    }
}
