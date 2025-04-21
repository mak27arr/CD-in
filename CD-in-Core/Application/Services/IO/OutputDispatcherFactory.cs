using Microsoft.Extensions.DependencyInjection;

namespace CD_in_Core.Application.Services.IO
{
    internal class OutputDispatcherFactory : IOutputDispatcherFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutputDispatcherFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public IOutputDispatcherService Create()
        {
            var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IOutputDispatcherService>();
        }
    }
}
