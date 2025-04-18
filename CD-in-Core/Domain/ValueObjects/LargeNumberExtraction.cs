using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class LargeNumberExtraction : BaseExtraction
    {
        public IValueCondition<IElement> Condition { get; set; } = default!;
    }
}
