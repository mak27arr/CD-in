namespace CD_in_Core.Domain.Models.Sequences
{
    public class Element : IElement
    {
        public static Element Default { get; } = new Element(int.MinValue, int.MinValue, int.MinValue);

        public Element(int key, int displayKey, int value)
        {
            Key = key;
            DisplayKey = displayKey;
            Value = value;
        }

        public int Key { get; }

        public int DisplayKey { get; set; }

        public int Value { get; }

        public IElement Clone()
        {
            return new Element(Key, DisplayKey, Value);
        }
    }
}
