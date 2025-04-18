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

        public ISequence ProcessBlock(PoolArray<byte> digits)
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

        private ISequence CalculateTargetDelta()
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

        public ISequence CalculateDelta(List<int> indexesAndDeltas)
        {
            if (indexesAndDeltas is null or { Count: 0 })
                return default;

            var sequence = _pool.Get();
            sequence.Add(indexesAndDeltas[0], indexesAndDeltas[0]);

            for (int i = 1; i < indexesAndDeltas.Count; i++)
            {
                var previous = indexesAndDeltas[i - 1];
                sequence.Add(indexesAndDeltas[i], indexesAndDeltas[i] - previous);
            }

            return sequence;
        }
    }
}
