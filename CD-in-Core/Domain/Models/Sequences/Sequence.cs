using System.Collections;

namespace CD_in_Core.Domain.Models.Sequences
{
    internal class Sequence : ISequence
    {
        internal Dictionary<int, int> _digits;

        public int Count => _digits.Count;

        internal Sequence(int size = 1)
        {
            _digits = new Dictionary<int, int>(size);
        }

        public void Add(int index, int value)
        {
            _digits[index] = value;
        }

        public void Add(KeyValuePair<int, int> element)
        {
            _digits[element.Key] = element.Value;
        }

        public void Clear()
        {
            _digits.Clear();
        }

        public int GetNext(int index)
        {
            return _digits.TryGetValue(index + 1, out var element) ? element : Element.Default.Value;
        }

        public int GetPrevious(int index)
        {
            return _digits.TryGetValue(index - 1, out var element) ? element : Element.Default.Value;
        }


        public void SetCapacity(int count)
        {
            _digits.EnsureCapacity(count);
        }

        #region IEnumerable<Element>

        public IEnumerator<KeyValuePair<int, int>> GetEnumerator()
        {
            return _digits.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
