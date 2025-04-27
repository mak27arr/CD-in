using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public class ConstantTransformer : IValueTransformer<IElement>
    {
        private readonly int _replacement;

        public ConstantTransformer(int replacement)
        {
            _replacement = replacement;
        }

        public IElement Transform(IElement originalValue)
        {
            return new Element(originalValue.Key, originalValue.DisplayKey, _replacement);
        }
    }
}
