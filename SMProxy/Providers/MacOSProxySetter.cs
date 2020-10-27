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
        private const string TEMPLATE_FILE = "command_templates\\setproxy_mac.sh";
        private string _template;

        public MacOSProxySetter() : base()
        {

        }

        protected override string TemplateFilePath => TEMPLATE_FILE;
        #region implementations

        public void SetAuto()
        {
            throw new NotImplementedException();
        }

        protected override string GetPlatformExecutableTempFile()
        {
            return $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.sh";
        }
        #endregion
    }
}
