using System;
using System.Linq;

namespace MedLab
{
    public sealed class DnsGhostLib
    {   
        public static string GetIpAddress(out bool cached)
        {
            return (new DnsGhostLib()).GetIpAddressInternal(out cached);
        }
 
        public static string GetIpAddress(string url, out bool cached)
        {
            return (new DnsGhostLib()).GetIpAddressInternal(url, out cached);
        }
 
        public static string GetIpAddress(string url)
        {
            return (new DnsGhostLib()).GetIpAddressInternal(url);
        }
 
        public static string GetIpAddress()
        {
            return (new DnsGhostLib()).GetIpAddressInternal();
        }
 
        #region IMPLEMENTATION
        
 
        string url;
        string fileName;
        protected DnsGhostLib()
        {
            this.url = System.Configuration.ConfigurationManager.AppSettings["dnsGhostUrl"];
 
            //<appSettings>
            //  <add key="dnsGhostUrl" value="http://ownmeca0.w15.wh-2.com/DnsGhost/dns/retrieve/username/computername"/>
            //</appSettings>
            if (this.url == null)
                throw new System.Configuration.ConfigurationErrorsException("Invalid configuraion. Please add the dnsGhostUrl to the application configuration.");
            fileName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "dnsGhost.txt";
        }
 
        private string GetIpAddressInternal(string url)
        {
            bool cached;
            return GetIpAddressInternal(url, out cached);
        }
 
        private string GetIpAddressInternal()
        {
            bool cached;
            return GetIpAddressInternal(this.url, out cached);
        }
 
        private string GetIpAddressInternal(out bool cached)
        {        
            return GetIpAddressInternal(this.url, out cached);
        }
        
        private string GetIpAddressInternal(string url, out bool cached)
        {
            var wc = new System.Net.WebClient() { Proxy = null };
            try
            {
                var ipAddress = wc.DownloadString(url);
                SaveInfoToFile(ipAddress);
                cached = false;
                return ipAddress;
 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(System.Globalization.CultureInfo.InvariantCulture, "exception {0}", ex.Message));
                cached = true;
                return GetSavedInfoFromFile();
            }
        }
 
        private string GetSavedInfoFromFile()
        {
            if (System.IO.File.Exists(this.fileName))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(this.fileName))
                {
                    return file.ReadLine();
                }
            }
            return null;
        }
 
        private void SaveInfoToFile(string info)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(this.fileName, false))
            {
                file.WriteLine(info);
            }
        }
 
        #endregion
    }
 
}