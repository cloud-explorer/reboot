namespace Projects.Common.Contracts
{
    public interface IServiceFactory
    {
        T GetService<T>() where T : IServiceContract;
    }
}
