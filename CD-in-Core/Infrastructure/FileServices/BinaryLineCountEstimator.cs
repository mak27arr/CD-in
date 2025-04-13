namespace CD_in_Core.Infrastructure.FileServices
{
    internal class BinaryLineCountEstimator : ILineCountEstimator
    {
        private int _bytesPerLine = 3;

        public long EstimateLineCount(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found.", filePath);

            return fileInfo.Length / _bytesPerLine;
        }
    }
}
