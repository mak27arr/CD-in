﻿namespace CD_in_Core.Domain.Select
{
    public class BaseExtractionRule : IExtractionRule
    {
        public int ExecutionOrder { get; init; }

        public required ExtractionRetentionPolicy RetentionPolicy { get; init; }
    }
}
