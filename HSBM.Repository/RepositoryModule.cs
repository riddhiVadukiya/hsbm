using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace HSBM.Repository
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(spec => spec.FromThisAssembly()
                                  .SelectAllClasses()
                                  .BindAllInterfaces()
                                  .Configure(config => config.InRequestScope()));
        }
    }
}
