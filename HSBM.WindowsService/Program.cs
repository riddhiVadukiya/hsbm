using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace HSBM.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new HimalayanFarmstayService() 
            };
            ServiceBase.Run(ServicesToRun);

            //if (Environment.UserInteractive)
            //{
            //    RunInteractive(ServicesToRun);
            //}
            //else
            //{
            //    ServiceBase.Run(ServicesToRun);
            //}
        }

        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
            BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}...", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("{0} Started", service.ServiceName);
            }
        }
    }
}
