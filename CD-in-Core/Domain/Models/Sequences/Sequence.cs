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

        [Obsolete]
        public void Add(int key, int value)
        {
            Add(key, key, value);
        }

        public void Add(int key, int displayKey, int value)
        {
            _digits[key] = new Element(key, displayKey, value);
        }

        public void Remove(IElement item)
        {
            _digits.Remove(item.Key);
        }

        public void Clear()
        {
            _digits.Clear();
        }

        public IElement GetNext(IElement item)
        {
            return _digits.TryGetValue(item.Key + 1, out var element) 
                ? element 
                : _digits.TryGetValue(_digits.Keys.FirstOrDefault(k => k > item.Key), out var nextValue) ? nextValue : Element.Default;
        }

        public IElement GetPrevious(IElement item)
        {
            return _digits.TryGetValue(item.Key - 1, out var element) 
                ? element
                : _digits.TryGetValue(_digits.Keys.LastOrDefault(k => k < item.Key), out var prvValue) ? prvValue : Element.Default; ;
        }

        public void SetCapacity(int count)
        {
            _digits.EnsureCapacity(count);
        }

        public void ReindexDisplayKeys()
        {
            int index = 1;
            foreach (var element in _digits.Values)
            {
                element.DisplayKey = index++;
            }
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
