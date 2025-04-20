using System.Collections;

namespace CD_in_Core.Domain.Models.Sequences
{
    internal class Sequence : ISequence
    {
        internal Dictionary<int, IElement> _digits;

        public int Count => _digits.Count;

        internal Sequence(int size = 1)
        {
            _digits = new Dictionary<int, IElement>(size);
        }

        public void Add(IElement element)
        {
            _digits[element.Key] = element;
        }

        public void Add(int key, int value)
        {
            _digits[key] = new Element(key, value);
        }

        public void Clear()
        {
            _digits.Clear();
        }

        public IElement GetNext(IElement item)
        {
            return _digits.TryGetValue(item.Key + 1, out var element) ? element : Element.Default;
        }

        public IElement GetPrevious(IElement item)
        {
            return _digits.TryGetValue(item.Key - 1, out var element) ? element : Element.Default;
        }

        #region IEnumerable<Element>

        public IEnumerator<IElement> GetEnumerator()
        {
            return _digits.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
