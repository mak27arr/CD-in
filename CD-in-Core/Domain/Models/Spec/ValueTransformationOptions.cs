using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Replacement
{
    public class ValueTransformationOptions : BaseOption
    {
        public ISequenceCondition<KeyValuePair<int, int>> Specification { get; set; } = default!;

        public IValueTransformer<int> ReplacementStrategy { get; set; } = default!;
    }
}
