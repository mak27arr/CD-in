using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Domain.Conditions
{
    public class ConstantTransformer : IValueTransformer<KeyValuePair<int, int>>
    {
        private readonly int _replacement;

        public ConstantTransformer(int replacement)
        {
            _replacement = replacement;
        }

        public KeyValuePair<int, int> Transform(KeyValuePair<int, int> originalValue)
        {
            return new KeyValuePair<int, int>(originalValue.Key, _replacement);
        }
    }
}
