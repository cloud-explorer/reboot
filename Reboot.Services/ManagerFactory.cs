using Projects.Common.Contracts;

namespace Projects.Reboot.Common
{
    public class ManagerFactory : IManagerFactory
    {
        public T GetManager<T>() where T : IManager
        {
            return ObjectBase.Container.Resolve<T>(typeof(T));
        }
    }
}