﻿namespace CD_in_Core.Infrastructure.FileServices
{
    public interface IFileReadProgressTracker
    {
        void Initialize(string filePath);
        double GetProgressPercentage(long currentReadLines);
    }
}
