using log4net;
using SMProxy.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SMProxy.Providers
{
    public class WindowsProxySetter : ProxySetter, IProxySetter
    {
        protected override string TemplateFilePath { get => Path.Combine("command_templates", "setproxy_windows.bat"); }
        public WindowsProxySetter(ILog logger) : base(logger)
        {
            
        }
        protected override string GetPlatformExecutableTempFile()
        {
            return $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.bat";
        }
    }
}
