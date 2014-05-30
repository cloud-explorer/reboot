namespace Projects.Common.Contracts
{
    public interface IManagerFactory
    {
        T GetManager<T>() where T : IManager;
    }
}
