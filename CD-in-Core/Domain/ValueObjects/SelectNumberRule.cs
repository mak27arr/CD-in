using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class SelectNumberRule : BaseExtractionRule
    {
        public required IValueCondition<IElement> Condition { get; set; }
    }
}
