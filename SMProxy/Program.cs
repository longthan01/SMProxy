using SMProxy.Abstracts;
using SMProxy.Loaders;
using SMProxy.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SMProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            SetAuto();
        }
        private static void SetAuto()
        {
            var proxyListProviders = MapperProvider.Load("mappers").ToList();
            Random d = new Random();
            int index = d.Next(0, proxyListProviders.Count - 1);
            IProxyListProvider provider = proxyListProviders[index];
            List<Proxy> proxies = provider.GetProxyList().ToList();
            int proxyIndex = d.Next(0, proxies.Count - 1);
            Proxy proxy = proxies[proxyIndex];
            var proxySetter = GetProxySetter();
            if (proxySetter != null)
            {
                proxySetter.Set(proxy.Host, proxy.Port);
            }
        }
        private static IProxySetter GetProxySetter()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (isWindows)
            {
                return new WindowsProxySetter();
            }
            else
            {
                if (isOSX)
                {
                    return new MacOSProxySetter();
                }
            }
            return null;
        }
    }
}
