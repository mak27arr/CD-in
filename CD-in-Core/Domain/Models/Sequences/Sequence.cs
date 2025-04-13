namespace CD_in_Core.Domain.Models.Sequences
{
    public class Sequence
    {
        public Dictionary<int, int> Digits { get; internal set; } = new();

        internal void Add(int key, int value)
        {
            Digits.Add(key, value);
        }

        internal void Clear()
        {
            Digits.Clear();
        }
    }
}
