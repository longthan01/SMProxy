using SMProxy.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMProxy.Providers
{
    public class WindowsProxySetter : IProxySetter
    {
        public void Set(string server, string port)
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", 1);
            registry.SetValue("ProxyServer", "127.0.0.1:8080");
        }

        public void SetAuto()
        {
            throw new NotImplementedException();
        }
    }
}
