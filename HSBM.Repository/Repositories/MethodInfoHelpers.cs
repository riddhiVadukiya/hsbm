using System.Reflection;

namespace HSBM.Repository.Repositories
{
    public class MethodInfoHelpers
    {
        public static MethodInfo MethodContains { get; private set; }

        static MethodInfoHelpers()
        {
            MethodContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        }
    }
}
