using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace DnsGhost
{
    public partial class DnsGhostService : ServiceBase
    {
        public DnsGhostService()
        {
            InitializeComponent();
        }

        Timer timer;
        string url;

        protected override void OnStart(string[] args)
        {            
            this.url = ConfigurationManager.AppSettings.Get("url");
            int interval = 3600;            
            Int32.TryParse(ConfigurationManager.AppSettings.Get("frequencyInSeconds"), out interval);
            timer = new Timer(TimerCallback, null, 0, (int)TimeSpan.FromSeconds(interval).TotalMilliseconds);
        }

        public void DebugOnStart()
        {
            this.OnStart(null);
        }

        private void TimerCallback(object state)
        {
            WebClient wc = new WebClient();
            try
            {
                var confirmation = wc.DownloadString(new Uri(url, UriKind.Absolute));
                System.Diagnostics.Trace.WriteLine(confirmation);
            }
            catch(Exception e)
            {
                System.Diagnostics.Trace.WriteLine("Exception " + e.Message);
            }
        }

        protected override void OnStop()
        {
            timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }
    }
}
