using log4net;
using Microsoft.VisualBasic.CompilerServices;
using SMProxy.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SMProxy.Providers
{
    public class WindowsProxySetter : ProxySetter, IProxySetter
    {
        private string TemplateFilePath { get => Path.Combine("command_templates", "setproxy_windows.bat"); }
        private string RemoveProxyTemplateFilePath { get => Path.Combine("command_templates", "removeproxy_windows.bat"); }
        public WindowsProxySetter(ILog logger) : base(logger)
        {

        }
        protected override string GetPlatformExecutableTempFile()
        {
            return $"{Guid.NewGuid()}-{DateTime.Now.Ticks}.bat";
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
                    Logger?.Error($"Process exited with exit code ${proc.ExitCode}");
                    throw new Exception($"Process exited with an error code: {proc.ExitCode}");
                }
            }
            finally
            {
                // delete temp file 
                File.Delete(tempFileName);
                Logger?.Info("DONE");
            }
        }

        public void Set(string server, string port)
        {
            if (!this.ValidateTemplateFile(TemplateFilePath))
            {
                return;
            }
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
                FileName = tempFileName,
                RedirectStandardOutput = false,
                CreateNoWindow = true,
            };
            Process proc = new Process() { StartInfo = psi };
            try
            {
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                {
                    Logger?.Error($"Process exited with exit code ${proc.ExitCode}");
                    throw new Exception($"Process exited with an error code: {proc.ExitCode}");
                }
            }
            finally
            {
                // delete temp file 
                File.Delete(tempFileName);
                Logger?.Info("DONE");
            }
        }
    }
}
