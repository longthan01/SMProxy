using log4net;
using log4net.Config;
using SMProxy.Abstracts;
using SMProxy.Loaders;
using SMProxy.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SMProxy
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void LoadLog4Net()
        {
            // Load configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        static void Main(string[] args)
        {
            // load log4net configuration
            LoadLog4Net();
            // do it's job
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
                return new WindowsProxySetter(logger);
            }
            else
            {
                if (isOSX)
                {
                    return new MacOSProxySetter(logger);
                }
            }
            return null;
        }
    }
}
