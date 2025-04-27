namespace CD_in_Core.Domain.Select
{
    public interface IExtractionRule
    {
        ExtractionRetentionPolicy RetentionPolicy { get; init; }
    }
}
