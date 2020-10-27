using SMProxy.Abstracts;
using SMProxy.Providers;
using System;
using System.Runtime.InteropServices;

namespace SMProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxySetter = GetProxySetter();
            if(proxySetter != null)
            {
                proxySetter.Set("1.1.1.1", "8080");
            }
        }
        private static IProxySetter GetProxySetter()
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if(isWindows)
            {
                return new WindowsProxySetter();
            }
            else
            {
                if(isOSX)
                {
                    return new MacOSProxySetter();
                }
            }
            return null;
        }
    }
}
