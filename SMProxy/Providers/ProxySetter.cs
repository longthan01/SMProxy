using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Diagnostics;
using System.IO;

namespace SMProxy.Providers
{
    public abstract class ProxySetter
    {
        protected ILog Logger { get; }

        public ProxySetter(ILog logger)
        {
            Logger = logger;
        }
        protected bool ValidateTemplateFile(string file)
        {
            if(!File.Exists(file))
            {
                Logger.Error($"Template file {file} does not exit.");
                return false;
            }
            return true;
        }
        protected abstract string GetPlatformExecutableTempFile();
    }
}