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
        public List<Website> GetAllModifiedWebsites(int userId)
        {
            List<Website> newList = new List<Website>();
            List<Website> oldList = GetAllSubscribedWebsites(userId);
            foreach (var site in oldList)
            {
                string hash = GetMD5Hash(site.Address);
                if(site.Hash != hash)
                {
                    site.Hash = hash;
                    newList.Add(site);
                }
            }
            return newList;
        }

        private List<Website> GetAllSubscribedWebsites(int userId)
        {
            List<Website> list = new List<Website>();
            return list;
        }

        private string GetMD5Hash(string URL)
        {
            WebClient client = new WebClient();
            string source = client.DownloadString(new Uri(URL, UriKind.Absolute));
            MD5 md5 = MD5.Create();
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(source);
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
