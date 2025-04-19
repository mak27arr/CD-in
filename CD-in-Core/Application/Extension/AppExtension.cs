using CD_in_Core.Application.Services.DeltaIndex;
using CD_in_Core.Application.Services.Interfaces;
using CD_in_Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using CD_in_Core.Infrastructure.Extension;
using CD_in_Core.Application.Services.Sequences;
using CD_in_Core.Application.Pool;
using Microsoft.Extensions.ObjectPool;
using CD_in_Core.Application.Services.Interfaces.Sequences;
using CD_in_Core.Application.Services.IO;

namespace CD_in_Core.Application.Extension
{
    public static class AppExtension
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection service)
        {
            service.RegisterAppInfrastructureServices();
            service.AddTransient<IDeltaIndexService, DeltaIndexService>();
            service.AddTransient<IDeltaIndexTextFileReader, DeltaIndexTextFileReader>();
            service.AddTransient<ISequenceExtractionService, SequenceExtractionService>();
            service.AddTransient<IReplacementService, BeneficialReplacementService>();
            service.AddTransient<ISubSequenceExtractorService, SubSequenceExtractorService>();
            service.AddTransient<ISequenceProcessingService, SequenceProcessingService>();
            service.AddTransient<IDirectoryProcessingService, DirectoryProcessingService>();
            service.AddTransient<IMainProcessingService, MainProcessingService>();
            service.AddTransient<IOutputDispatcherService, OutputDispatcherService>();
            service.AddTransient<IInputDispatcherService, InputDispatcherService>();
            service.AddSingleton<ISequencePool>(provider =>
            {
                var poolProvider = new DefaultObjectPoolProvider();
                var poolPolicy = new SequencePooledObjectPolicy(size: 50000);
                var innerPool = poolProvider.Create(poolPolicy);
                var wrapper = new SequencePool(innerPool);
                return wrapper;
            });
            return service;
        }
    }
}
