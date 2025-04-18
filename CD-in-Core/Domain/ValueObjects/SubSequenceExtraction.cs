using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class SubSequenceExtraction : BaseExtraction
    {
        public IValueCondition<IElement> Condition { get; init; } = default!;

        public int MinSequenceLength { get; init; }
    }
}
