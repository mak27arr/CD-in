using CD_in_Core.Domain.Models.Sequences;
using CD_in_Core.Domain.Select;

namespace CD_in_Core.Application.Services.Interfaces.Sequences
{
    internal interface IBeneficialReplacementService
    {
        ISequence PerformBeneficialReplacement(ISequence sequence, ValueTransformation options);
    }
}
