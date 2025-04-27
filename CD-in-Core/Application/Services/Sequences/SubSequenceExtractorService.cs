using CD_in_Core.Application.Pool;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Sequences
{
    internal class SubSequenceExtractorService : BaseProcessingService, ISubSequenceExtractorService
    {
        private readonly List<IElement> _currentSequence = new List<IElement>();
        private readonly ISequencePool _pool;

        public SubSequenceExtractorService(ISequencePool pool)
        {
            _pool = pool;
        }

        public ISequence ExstractSubSequence(ISequence sequence, SubSequenceExtractionRule options)
        {
            var resultSequence = _pool.Get();
            var statisfaedSequence = _pool.Get();
            _currentSequence.Clear();

            foreach (var element in sequence)
            {
                if (options.Condition.IsSatisfiedBy(element))
                {
                    _currentSequence.Add(element);
                }
                else
                {
                    if (CopySubSequenceToResult(options, resultSequence))
                        foreach (var statisfaedElement in _currentSequence)
                            statisfaedSequence.Add(statisfaedElement.Clone());

                    _currentSequence.Clear();
                }
            }

            if (CopySubSequenceToResult(options, resultSequence))
                foreach (var statisfaedElement in _currentSequence)
                    statisfaedSequence.Add(statisfaedElement.Clone());

            UpdateSource(sequence, statisfaedSequence, options.RetentionPolicy);

            return resultSequence;
        }

        private bool CopySubSequenceToResult(SubSequenceExtractionRule options, IPooledSequence resultSequence)
        {
            if (_currentSequence.Count < options.MinSequenceLength)
                return false;

            switch (options.Action)
            {
                case SubSequenceAction.Count:
                    CountSubSequenceElementAndAdd(options, resultSequence);
                    break;
                case SubSequenceAction.Exstract:
                    ExstractSubSequenceAndAdd(options, resultSequence);
                    break;
                default:
                    throw new NotImplementedException($"SubSequenceAction: {options.Action} not supported action");
            }

            return true;
        }

        private void CountSubSequenceElementAndAdd(SubSequenceExtractionRule options, IPooledSequence resultSequence)
        {
            var element = _currentSequence.FirstOrDefault()
                ?? throw new Exception("Can't extract subsequence from empty sequence");
            resultSequence.Add(element.Key, element.DisplayKey, _currentSequence.Count);
        }

        private void ExstractSubSequenceAndAdd(SubSequenceExtractionRule options, IPooledSequence resultSequence)
        {
            foreach (var element in _currentSequence)
            {
                resultSequence.Add(element.Clone());
            }
        }
    }
}
