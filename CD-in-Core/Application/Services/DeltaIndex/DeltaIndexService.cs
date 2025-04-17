using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Extension;

namespace CD_in_Core.Application.Services.DeltaIndex
{
    internal class DeltaIndexService : IDeltaIndexService
    {
        private readonly List<int> _onesIndexesAndDeltas = new();
        private readonly List<int> _zerosIndexesAndDeltas = new();

        public IEnumerable<IElement> ProcessBlock(PoolArray<byte> digits)
        {
            var index = 1;
            _onesIndexesAndDeltas.Clear();
            _zerosIndexesAndDeltas.Clear();

            foreach (var digit in digits)
            {
                if (digit == 0)
                {
                    _zerosIndexesAndDeltas.Add(index);
                }
                else if (digit == 1)
                {
                    _onesIndexesAndDeltas.Add(index);
                }

                index++;
            }

            return CalculateTargetDelta();
        }

        private Element CreateElementForIndex(int index, int value)
        {
            return new Element() { Key = index, Value = value };
        }

        private IEnumerable<IElement> CalculateTargetDelta()
        {
            if (_onesIndexesAndDeltas.Count < _zerosIndexesAndDeltas.Count)
            {
                return CalculateDelta(_onesIndexesAndDeltas);
            }
            else
            {
                return CalculateDelta(_zerosIndexesAndDeltas);
            }
        }

        public IEnumerable<IElement> CalculateDelta(List<int> indexesAndDeltas)
        {
            if (indexesAndDeltas is null or { Count: 0 })
                return Array.Empty<IElement>();

            var array = new Element[indexesAndDeltas.Count];
            array[0] = CreateElementForIndex(indexesAndDeltas[0], indexesAndDeltas[0]);

            for (int i = 1; i < indexesAndDeltas.Count; i++)
            {
                var previous = indexesAndDeltas[i - 1];
                array[i] = CreateElementForIndex(indexesAndDeltas[i], indexesAndDeltas[i] - previous);
            }

            return array;
        }
    }
}
