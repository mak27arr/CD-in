using System.Collections;

namespace CD_in_Core.Domain.Models.Sequences
{
    internal class Sequence : ISequence
    {
        internal Dictionary<int, Element> _digits;

        public int Count => _digits?.Count ?? 0;

        internal Sequence(int size = 1)
        {
            _digits = new Dictionary<int, Element>(size);
        }

        internal void Add(Element element)
        {
            _digits.Add(element.Key, element);
        }

        internal void Clear()
        {
            _digits.Clear();
        }

        public Element GetNext(Element item)
        {
            return _digits.TryGetValue(item.Key + 1, out var element) ? element : Element.Default;
        }

        public Element GetPrevious(Element item)
        {
            return _digits.TryGetValue(item.Key - 1, out var element) ? element : Element.Default;
        }

        #region IEnumerable<Element>

        public IEnumerator<Element> GetEnumerator()
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
