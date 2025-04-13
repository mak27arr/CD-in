namespace CD_in_Core.Domain.Models.Sequences
{
    public class Element
    {
        public Element()
        {
        }

        public Element(int index, int value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; set; }
        public int Value { get; set; }
    }
}
