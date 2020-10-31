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

        private string SetProxyTemplateFilePath => Path.Combine("command_templates", "setproxy_mac.sh");
        private string RemoveProxyTemplateFilePath => Path.Combine("command_templates", "removeproxy_mac.sh");
        #region implementations

        protected override string GetPlatformExecutableTempFile()
        {
            return $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.sh";
        }
        #endregion
        public void Set(string server, string port)
        {
            this.Logger?.Info($"Setting proxy: {server}:{port}");
            string template = File.ReadAllText(this.SetProxyTemplateFilePath);
            string command = template
                .Replace("<HOST>", server)
                .Replace("<PORT>", port);
           
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
                Logger?.Info($"Running command: {command}");
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                {
                    Logger?.Error($"Process existed with exit code {proc.ExitCode}");
                    throw new Exception($"Process exited with an error code: {proc.ExitCode}");
                }
            }
            finally
            {
                Logger?.Info("DONE");
            }
        }

        public void Remove()
        {
            this.Logger?.Info($"Removing proxy");
            if (!this.ValidateTemplateFile(this.RemoveProxyTemplateFilePath))
            { return; }
            string template = File.ReadAllText(this.RemoveProxyTemplateFilePath);
            string command = template;
            // create temporary file
            string tempFileName = Path.Combine(Directory.GetCurrentDirectory(), GetPlatformExecutableTempFile());
            File.WriteAllText(tempFileName, command);
            // start process
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = tempFileName,
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
