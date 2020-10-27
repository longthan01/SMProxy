using SMProxy.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SMProxy.Providers
{
    public class MacOSProxySetter : IProxySetter
    {
        private const string TEMPLATE_FILE = "command_templates\\setproxy_mac.sh";
        private string _template;

        public MacOSProxySetter()
        {
            if (!File.Exists(TEMPLATE_FILE))
            { throw new FileNotFoundException("Command template file did not found"); }
            this._template = File.ReadAllText(TEMPLATE_FILE);
        }
        #region implementations
        public void Set(string server, string port)
        {
            string command = _template
                .Replace("<HOST>", server)
                .Replace("<PORT>", port);
            // create temporary file
            string tempFileName = $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.sh";
            File.WriteAllText(tempFileName, command);
            // start process
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = tempFileName,
            };
            Process proc = new Process() { StartInfo = psi };
            proc.Start();
            if(proc.ExitCode != 0)
            {
                throw new Exception($"Process exited with an error code: {proc.ExitCode}");
            }
            // delete temp file 
            File.Delete(tempFileName);
        }

        public void SetAuto()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
