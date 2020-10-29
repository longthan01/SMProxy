using SMProxy.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SMProxy.Loaders
{
    public class MapperProvider
    {
        public static IEnumerable<IProxyListProvider> Load(string pluginDir)
        {
            var pluginFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), pluginDir);
            var injectedDlls = new DirectoryInfo(pluginFolder).GetFiles("SM.Proxy.*.dll").Select(x => x.FullName);
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var item in injectedDlls)
            {
                assemblies.Add(Assembly.LoadFile(item));
            }
            List<IProxyListProvider> providers = new List<IProxyListProvider>();
            foreach (Assembly ass in assemblies)
            {
                foreach (Type type in ass.GetTypes())
                {
                    foreach (var itf in type.GetInterfaces())
                    {
                        if (itf == typeof(IProxyListProvider))
                        {
                            IProxyListProvider mapperProvider = Activator.CreateInstance(type) as IProxyListProvider;
                            providers.Add(mapperProvider);
                        }
                    }
                }
            }
            return providers;
        }
    }
}
