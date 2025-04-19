using CD_in_Core.Domain.Conditions;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Select
{
    public class NumberExtractionRule : BaseExtractionRule
    {
        public required IValueCondition<IElement> Condition { get; set; }
    }
}
