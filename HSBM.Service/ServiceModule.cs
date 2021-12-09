using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;


namespace HSBM.Service
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(spec => spec.FromThisAssembly()
                                  .SelectAllClasses().EndingWith("Service")
                                  .BindAllInterfaces()
                                  .Configure(config => config.InRequestScope()));
        }
    }
}
