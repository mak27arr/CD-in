using CD_in_Core.Domain.Models.Replacement;
using CD_in_Core.Domain.Models.Sequences;

namespace CD_in_Core.Application.Services.Interfaces
{
    internal interface IBeneficialReplacementService
    {
        ISequence PerformBeneficialReplacement(ISequence sequence, ValueTransformationOptions options);
    }
}
