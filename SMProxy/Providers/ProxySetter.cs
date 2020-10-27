using System;
using System.Diagnostics;
using System.IO;

namespace SMProxy.Providers
{
    public abstract class ProxySetter
    {
        protected abstract string TemplateFilePath { get; }
        private string _template;
        public ProxySetter()
        {
            if (!File.Exists(TemplateFilePath))
            { throw new FileNotFoundException("Command template file did not found"); }
            this._template = File.ReadAllText(TemplateFilePath);
        }
        public virtual void Set(string server, string port)
        {
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
                    throw new Exception ($"Process exited with an error code: {proc.ExitCode}");
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