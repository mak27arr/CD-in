using CD_in_Core.Domain.Models.Specification;

namespace CD_in_Core.Domain.Models.Replacement
{
    public class ConstantTransformer : IValueTransformer<int>
    {
        private readonly int _replacement;

        public ConstantTransformer(int replacement)
        {
            _replacement = replacement;
        }

        public int Transform(int originalValue)
        {
            return _replacement;
        }
    }
}
