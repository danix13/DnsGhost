using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace DnsGhost
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
                new DnsGhostService() 
            };

            // test debugging of OnStart()
            //(new DnsGhostService()).DebugOnStart();

            ServiceBase.Run(ServicesToRun);
        }
    }
}
