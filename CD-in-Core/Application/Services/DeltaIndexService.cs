using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services
{
    public class DeltaIndexService : IDeltaIndexService
    {
        private readonly LinkedList<int> _onesIndexes = new();
        private readonly LinkedList<int> _zerosIndexes = new();
        private int _onesCount = 0;
        private int _zerosCount = 0;

        public void ProcessBlock(IEnumerable<int> digits, int globalOffset)
        {
            var block = digits.ToList();

            foreach (var digit in block)
            {
                if (digit == 0) 
                    _zerosCount++;
                else if (digit == 1) 
                    _onesCount++;
            }

            for (int i = 0; i < block.Count; i++)
            {
                if (block[i] == 1)
                    _onesIndexes.AddLast(globalOffset + i);
                else if (block[i] == 0)
                    _zerosIndexes.AddLast(globalOffset + i);
            }
        }

        public DeltaIndexResult GetResult()
        {
            GetBiggestTargetDelta(out var deltas, out var targetDigit);

            return new DeltaIndexResult
            {
                TargetDigit = targetDigit,
                Indexes = targetDigit == 1 ? _onesIndexes.ToList() : _zerosIndexes.ToList(),
                Deltas = deltas
            };
        }

        public void Reset()
        {
            _onesIndexes.Clear();
            _zerosIndexes.Clear();
            _onesCount = 0;
            _zerosCount = 0;
        }

        private void GetBiggestTargetDelta(out List<int> deltas, out int targetDigit)
        {
            if (_onesCount < _zerosCount)
            {
                targetDigit = 1;
                deltas = GetDeltas(_onesIndexes);
            }
            else
            {
                targetDigit = 0;
                deltas = GetDeltas(_zerosIndexes);
            }
        }

        private List<int> GetDeltas(LinkedList<int> indexes)
        {
            var deltas = new List<int>();

            var current = indexes.First;
            var previous = current;

            while (current != null && current.Next != null)
            {
                deltas.Add(current.Next.Value - current.Value);
                current = current.Next;
            }

            return deltas;
        }
    }
}
