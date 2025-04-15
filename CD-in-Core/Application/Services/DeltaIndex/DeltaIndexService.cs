using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Extension;

namespace CD_in_Core.Application.Services.DeltaIndex
{
    internal class DeltaIndexService : IDeltaIndexService
    {
        private readonly List<Element> _onesIndexesAndDeltas = new();
        private readonly List<Element> _zerosIndexesAndDeltas = new();

        public IEnumerable<Element> ProcessBlock(PoolArray<byte> digits, int globalOffset)
        {
            var index = 0;
            _onesIndexesAndDeltas.Clear();
            _zerosIndexesAndDeltas.Clear();

            foreach (var digit in digits)
            {
                if (digit == 0)
                {
                    _zerosIndexesAndDeltas.Add(CreateElementForIndex(globalOffset, index));
                }
                else if (digit == 1)
                {
                    _onesIndexesAndDeltas.Add(CreateElementForIndex(globalOffset, index));
                }

                index++;
            }

            CalculateTargetDelta(out var deltas);

            return deltas;
        }

        private Element CreateElementForIndex(int globalOffset, int index)
        {
            return new Element(globalOffset + index, index);
        }

        private void CalculateTargetDelta(out IEnumerable<Element> deltas)
        {
            if (_onesIndexesAndDeltas.Count < _zerosIndexesAndDeltas.Count)
            {
                deltas = CalculateDelta(_onesIndexesAndDeltas);
            }
            else
            {
                deltas = CalculateDelta(_zerosIndexesAndDeltas);
            }
        }

        public IEnumerable<Element> CalculateDelta(List<Element> indexesAndDeltas)
        {
            if (indexesAndDeltas is null or { Count: 0 })
                return Array.Empty<Element>();

            for (int i = 1; i < indexesAndDeltas.Count; i++)
            {
                var previous = indexesAndDeltas[i - 1];
                indexesAndDeltas[i].Value = indexesAndDeltas[i].Key - previous.Key;
            }

            return indexesAndDeltas;
        }
    }
}
