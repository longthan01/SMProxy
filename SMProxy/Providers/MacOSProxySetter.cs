using log4net;
using SMProxy.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SMProxy.Providers
{
    public class MacOSProxySetter : ProxySetter, IProxySetter
    {
        public MacOSProxySetter(ILog logger) : base(logger)
        {

        }

        protected override string TemplateFilePath => Path.Combine("command_templates", "setproxy_mac.sh");
        #region implementations

        protected override string GetPlatformExecutableTempFile()
        {
            return $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.sh";
        }
        #endregion
    }
}
