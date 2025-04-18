using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SubSequenceExtractorService : ISubSequenceExtractorService
    {
        private readonly List<IElement> _currentSequence = new List<IElement>();
        private readonly ISequencePool _pool;

        public SubSequenceExtractorService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence ExstractSequence(ISequence sequence, SubSequenceExtraction options)
        {
            var resultSequence = _pool.Get();
            _currentSequence.Clear();

            foreach (var element in sequence)
            {
                if (options.Condition.IsSatisfiedBy(element))
                {
                    _currentSequence.Add(element);
                }
                else
                {
                    CopySubSequenceToResult(options, resultSequence);
                    _currentSequence.Clear();
                }
            }

            CopySubSequenceToResult(options, resultSequence);

            return resultSequence;
        }

        private void CopySubSequenceToResult(SubSequenceExtraction options, IPooledSequence resultSequence)
        {
            if (_currentSequence.Count >= options.MinSequenceLength)
            {
                foreach (var element in _currentSequence)
                {
                    resultSequence.Add(element);
                }
            }
        }
    }
}
