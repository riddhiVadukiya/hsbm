using HSBM.Common.Azure;
using HSBM.Common.Contracts;
using Ninject.Modules;

namespace HSBM.Common
{
    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IConfigurationProvider>().To<AzureConfigurationProvider>().InSingletonScope();
        }
    }
}
