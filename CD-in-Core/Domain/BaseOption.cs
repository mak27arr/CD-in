using CD_in_Core.Domain.Models;

namespace CD_in_Core.Domain
{
    public class BaseOption : IOptions
    {
        public int ExecutionOrder { get; init; }
    }
}
