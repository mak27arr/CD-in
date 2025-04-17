using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models
{
    public class LargeNumberExtractionOptions : BaseOption
    {
        public IValueCondition<int> Condition { get; set; } = default!;
    }
}
