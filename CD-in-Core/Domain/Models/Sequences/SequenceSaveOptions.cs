﻿namespace CD_in_Core.Domain.Models.Sequences
{
    public record SequenceSaveOptions
    {
        public required string FileName { get; set; }

        public required string FilePath { get; set; }
    }
}