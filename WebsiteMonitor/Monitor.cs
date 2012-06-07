using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using System.Net;
using System.Security.Cryptography;

namespace WebsiteMonitor
{
    public class Monitor
    {
        public static List<MonitoredWebsite> FilterAllModifiedWebsites(List<MonitoredWebsite> list)
        {
            List<MonitoredWebsite> newList = new List<MonitoredWebsite>();
            foreach (var site in list)
            {
                string hash = GetMD5Hash(site.Id);
                if (site.HashedContent != hash)
                {
                    newList.Add(site);
                }
            }
            return newList;
        }

        public static string GetMD5Hash(string URL)
        {
            WebClient client = new WebClient();
            string source;
            try
            {
                source = client.DownloadString(new Uri(URL, UriKind.Absolute));
            }
            catch (WebException)
            {
                source = String.Empty;
                return source;
            }
            
            MD5 md5 = MD5.Create();
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
            byte[] hash = md5.ComputeHash(sourceBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
