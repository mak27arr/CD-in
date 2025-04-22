using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class ValueTransformationRule : BaseExtractionRule
    {
        public required ISequenceCondition<KeyValuePair<int, int>> Specification { get; set; }

        public required IValueTransformer<KeyValuePair<int, int>> ReplacementStrategy { get; set; }
    }
}
