using CD_in_Core.Infrastructure.FileServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace CD_in_Core.Infrastructure.FileServices
{
    internal class BinaryLineCountEstimator : ILineCountEstimator
    {
        private readonly ILogger<BinaryLineCountEstimator> _logger;
        private int _bytesPerLine = 3;

        public BinaryLineCountEstimator(ILogger<BinaryLineCountEstimator> logger)
        {
            _logger = logger;
        }

        public long EstimateLineCount(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found.", filePath);

            var linesCount =  fileInfo.Length / _bytesPerLine;
            _logger.LogInformation("File {0} have {1} lines", filePath, linesCount);

            return linesCount;
        }
    }
}
