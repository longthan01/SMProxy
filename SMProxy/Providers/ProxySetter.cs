using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Diagnostics;
using System.IO;

namespace SMProxy.Providers
{
    public abstract class ProxySetter
    {
        protected abstract string TemplateFilePath { get; }
        protected ILog Logger { get; }

        private string _template;
        public ProxySetter(ILog logger)
        {
            Logger = logger;
            if (!File.Exists(TemplateFilePath))
            {
                this.Logger?.Error($"Template file not found: {this.TemplateFilePath}");
                throw new FileNotFoundException("Command template file did not found");
            }
            this._template = File.ReadAllText(TemplateFilePath);
        }
        public virtual void Set(string server, string port)
        {
            this.Logger?.Info($"Setting proxy: {server}:{port}");
            string command = _template
                .Replace("<HOST>", server)
                .Replace("<PORT>", port);
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

        protected abstract string GetPlatformExecutableTempFile();
    }
}