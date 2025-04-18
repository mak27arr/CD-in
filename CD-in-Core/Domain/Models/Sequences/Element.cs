namespace CD_in_Core.Domain.Models.Sequences
{
    public class Element : IElement
    {
        public static Element Default { get; } = new Element(int.MinValue, int.MinValue);

        public Element(int key, int value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; set; }
        public int Value { get; set; }
    }
}
