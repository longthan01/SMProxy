using System;
using System.Collections.Generic;
using System.Text;

namespace SMProxy.Abstracts
{
    public interface IProxySetter
    {
        void Set(string server, string port);
        void Remove();
    }
}
