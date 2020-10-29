using System;
using System.Collections.Generic;
using System.Text;

namespace SMProxy.Providers
{
    public class Proxy
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public Metadata Metadata { get; set; }
    }
    public class Metadata
    {
        public string Country { get; set; }
        public string Ping { get; set; }
        public string Type { get; set; }
        public string Anonymity { get; set; }
    }
}
