namespace CD_in_Core.Domain.Models.Sequences
{
    public class Sequence
    {
        public Dictionary<int, int> Digits { get; private set; }

        public int Count => Digits?.Count ?? 0;

        internal Sequence(int size = 1)
        {
            Digits = new Dictionary<int, int>(size); 
        }

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
