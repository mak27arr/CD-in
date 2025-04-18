using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class ValueTransformation : BaseExtraction
    {
        public ISequenceCondition<IElement> Specification { get; set; } = default!;

        public IValueTransformer<IElement> ReplacementStrategy { get; set; } = default!;
    }
}
