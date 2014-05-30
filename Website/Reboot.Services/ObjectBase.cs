using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;

namespace Projects.Reboot.Common
{
    public abstract class ObjectBase
    {
        public static IWindsorContainer Container { get; set; }
    }
}
