namespace CD_in_Core.Domain.Models.Sequences
{
    public class Element : IElement
    {
        public static Element Default { get; } = new Element() { Key = int.MinValue, Value = int.MinValue };

        public int Key { get; set; }
        public int Value { get; set; }
    }
}
