using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models
{
    public class LargeNumberExtractionOptions : IOptions
    {
        public IValueCondition<int> Condition { get; set; } = default!;
    }
}
