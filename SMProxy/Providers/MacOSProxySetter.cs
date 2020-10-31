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
        public override void Set(string server, string port)
        {
            this.Logger?.Info($"Setting proxy: {server}:{port}");
            string template = File.ReadAllText(this.TemplateFilePath);
            string command = template
                .Replace("<HOST>", server)
                .Replace("<PORT>", port);
            // create temporary file
            string tempFileName = Path.Combine(Directory.GetCurrentDirectory(), GetPlatformExecutableTempFile());
            File.WriteAllText(tempFileName, command);
            // start process
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            Process proc = new Process() { StartInfo = psi };
            try
            {
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                {
                    throw new Exception($"Process exited with an error code: {proc.ExitCode}");
                }
            }
            finally
            {
                // delete temp file 
                File.Delete(tempFileName);
            }
            
        }
    }
}
