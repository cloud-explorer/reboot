using Projects.Common.Contracts;

namespace Projects.Reboot.Common
{
    public class ServiceFactory : IServiceFactory
    {
        public T GetService<T>() where T : IServiceContract
        {
            return ObjectBase.Container.Resolve<T>();
        }
    }
}
