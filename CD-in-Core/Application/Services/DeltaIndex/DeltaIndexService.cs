using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Extension;

namespace CD_in_Core.Application.Services.DeltaIndex
{
    internal class DeltaIndexService : IDeltaIndexService
    {
        private readonly List<int> _onesIndexesAndDeltas = new();
        private readonly List<int> _zerosIndexesAndDeltas = new();
        private readonly ISequencePool _pool;

        public DeltaIndexService(ISequencePool pool)
        {
            _pool = pool;
        }

        public IEnumerable<IElement> ProcessBlock(PoolArray<byte> digits)
        {
            _onesIndexesAndDeltas.Clear();
            _zerosIndexesAndDeltas.Clear();

            for (int i = 0; i < digits.Count; i++)
            {
                if (digits.Data[i] == 0)
                {
                    _zerosIndexesAndDeltas.Add(i);
                }
                else if (digits.Data[i] == 1)
                {
                    _onesIndexesAndDeltas.Add(i);
                }
            }

            return CalculateTargetDelta();
        }

        private Element CreateElementForIndex(int index, int value)
        {
            return new Element(index, value);
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

            var sequence = _pool.Get();
            sequence.Add(CreateElementForIndex(indexesAndDeltas[0], indexesAndDeltas[0]));

            for (int i = 1; i < indexesAndDeltas.Count; i++)
            {
                var previous = indexesAndDeltas[i - 1];
                sequence.Add(CreateElementForIndex(indexesAndDeltas[i], indexesAndDeltas[i] - previous));
            }

            return sequence;
        }
    }
}
