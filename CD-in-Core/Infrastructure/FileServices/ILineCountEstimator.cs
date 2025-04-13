namespace CD_in_Core.Infrastructure.FileServices
{
    interface ILineCountEstimator
    {
        long EstimateLineCount(string filePath);
    }
}
