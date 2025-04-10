using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Domain.Models;

namespace CD_in_Core.Application.Services
{
    public class SequenceExtractorService : ISequenceExtractorService
    {
        private readonly SequenceExtractionOptions _options;
        private readonly LinkedList<SequenceInfo> _sequences = new();

        private int _currentIndex = 0;
        private int _currentSequenceStart = -1;
        private int _currentSequenceLength = 0;

        public SequenceExtractorService(SequenceExtractionOptions options)
        {
            _options = options;
        }

        public void ProcessBlock(IEnumerable<int> digits, int globalOffset)
        {
            var list = digits.ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var value = list[i];
                var absoluteIndex = globalOffset + i;

                if (value == _options.TargetDigit)
                {
                    if (_currentSequenceLength == 0)
                        _currentSequenceStart = absoluteIndex;

                    _currentSequenceLength++;
                }
                else
                {
                    FinalizeSequence();
                }
            }
        }

        public SequenceExtractionResult GetResult()
        {
            FinalizeSequence();

            return new SequenceExtractionResult
            {
                Sequences = _sequences.ToList()
            };
        }

        public void Reset()
        {
            _sequences.Clear();
            _currentSequenceStart = -1;
            _currentSequenceLength = 0;
        }

        private void FinalizeSequence()
        {
            if (_currentSequenceLength >= _options.MinSequenceLength)
            {
                _sequences.AddLast(new SequenceInfo
                {
                    StartIndex = _currentSequenceStart,
                    Count = _currentSequenceLength
                });
            }

            _currentSequenceStart = -1;
            _currentSequenceLength = 0;
        }
    }
}
