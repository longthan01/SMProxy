using SMProxy.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SMProxy.Providers
{
    public class WindowsProxySetter : ProxySetter, IProxySetter
    {
        private const string TEMPLATE_FILE = "command_templates\\setproxy_windows.bat";
        private string _template;

        protected override string TemplateFilePath { get => TEMPLATE_FILE; }
        public WindowsProxySetter() : base()
        {

        }
        protected override string GetPlatformExecutableTempFile()
        {
            return $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.bat";
        }
    }
}
