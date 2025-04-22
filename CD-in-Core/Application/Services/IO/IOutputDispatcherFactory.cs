namespace CD_in_Core.Application.Services.IO
{
    internal interface IOutputDispatcherFactory
    {
        IOutputDispatcherService Create();
    }
}
