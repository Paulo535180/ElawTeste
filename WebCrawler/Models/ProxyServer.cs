using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class ProxyServer
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string Port { get; set; }
        public string Country { get; set; }
        public string Protocol { get; set; }
    }
}
