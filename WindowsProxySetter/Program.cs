using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsProxySetter
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameters = ParseParams(args);
            var proxyEnabled = parameters.ContainsKey("--proxy-enabled");
            var host = GetParam(parameters, "--host");
            var port = GetParam(parameters, "--port");
           
            string uri = $"{host}:{port}";
            if(string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
            {
                uri = "";
            }
            SetProxy(uri, proxyEnabled);
        }
        private static void SetProxy(string uri, bool enabled)
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            registry.SetValue("ProxyEnable", enabled ? 1 : 0);
            registry.SetValue("ProxyServer", uri);
        }
        private static void PrintMenu()
        {
            Console.WriteLine($"windowsproxysetter <params> <optional>");
            Console.WriteLine($"<params> can be:");
            Console.WriteLine($"    --host [host]");
            Console.WriteLine($"    --port [port]");
            Console.WriteLine($"optional");
            Console.WriteLine($"    --proxy-enabled");
        }
        private static string GetParam(Dictionary<string,string> parameters, string name)
        {
            if (!parameters.ContainsKey(name))
            {
                return null;
            }

            return parameters.FirstOrDefault(x => x.Key.Equals(name, StringComparison.OrdinalIgnoreCase)).Value;
        }
        private static Dictionary<string,string> ParseParams(string[] args)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; i++)
            {
                if (i + 1 < args.Length && !args[i+1].StartsWith("--"))
                {
                    parameters.Add(args[i], args[i + 1]);
                    i++;
                }
                else
                {
                    parameters.Add(args[i], "");
                }
            }
            return parameters;
        }
    }
}
