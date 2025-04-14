namespace CD_in_Core.Infrastructure.FileServices.Interfaces
{
    interface ILineCountEstimator
    {
        long EstimateLineCount(string filePath);
    }
}
