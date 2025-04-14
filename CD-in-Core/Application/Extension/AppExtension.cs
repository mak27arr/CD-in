using CD_in_Core.Application.Services.DeltaIndex;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using CD_in_Core.Infrastructure.Extension;
using CD_in_Core.Application.Services.Sequences;

namespace CD_in_Core.Application.Extension
{
    public static class AppExtension
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection service)
        {
            service.RegisterAppInfrastructureServices();
            service.AddTransient<IDeltaIndexService, DeltaIndexService>();
            service.AddTransient<IDeltaIndexProcessorService, DeltaIndexProcessorService>();
            service.AddTransient<ILargeNumberExtractionService, LargeNumberExtractionService>();
            service.AddTransient<IBeneficialReplacementService, BeneficialReplacementService>();
            service.AddTransient<ISubSequenceExtractorService, SubSequenceExtractorService>();
            service.AddTransient<IMainProcessingService, MainProcessingService>();
            service.AddTransient<IFolderProcessingService, FolderProcessingService>();

            return service;
        }
    }
}
