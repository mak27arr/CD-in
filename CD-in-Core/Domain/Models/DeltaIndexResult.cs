namespace CD_in_Core.Domain.Models
{
    public class DeltaIndexResult
    {
        public int TargetDigit { get; set; }
        public List<int> Indexes { get; set; } = new();
        public List<int> Deltas { get; set; } = new();
    }
}
