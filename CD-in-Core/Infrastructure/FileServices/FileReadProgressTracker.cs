namespace CD_in_Core.Infrastructure.FileServices
{
    internal class FileReadProgressTracker : IFileReadProgressTracker
    {
        private readonly ILineCountEstimator _lineCountEstimator;
        private long _totalLines;

        public FileReadProgressTracker(ILineCountEstimator lineCountEstimator)
        {
            _lineCountEstimator = lineCountEstimator;
        }

        public void Initialize(string filePath)
        {
            _totalLines = _lineCountEstimator.EstimateLineCount(filePath);
        }

        public double GetProgressPercentage(long currentReadLines)
        {
            if (_totalLines == 0)
                return 100;

            return Math.Min(100.0 * currentReadLines / _totalLines, 100.0);
        }
    }
}
