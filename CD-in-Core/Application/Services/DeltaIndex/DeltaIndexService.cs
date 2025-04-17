using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Extension;

namespace CD_in_Core.Application.Services.DeltaIndex
{
    internal class DeltaIndexService : IDeltaIndexService
    {
        private readonly List<IElement> _onesIndexesAndDeltas = new();
        private readonly List<IElement> _zerosIndexesAndDeltas = new();

        public IEnumerable<IElement> ProcessBlock(PoolArray<byte> digits)
        {
            var index = 1;
            _onesIndexesAndDeltas.Clear();
            _zerosIndexesAndDeltas.Clear();

            foreach (var digit in digits)
            {
                if (digit == 0)
                {
                    _zerosIndexesAndDeltas.Add(CreateElementForIndex(index));
                }
                else if (digit == 1)
                {
                    _onesIndexesAndDeltas.Add(CreateElementForIndex(index));
                }

                index++;
            }

            CalculateTargetDelta(out var deltas);

            return deltas;
        }

        private IElement CreateElementForIndex(int index)
        {
            return new Element() { Key = index };
        }

        private void CalculateTargetDelta(out IEnumerable<IElement> deltas)
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

        public IEnumerable<IElement> CalculateDelta(List<IElement> indexesAndDeltas)
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
