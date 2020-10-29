using System;
using System.Collections;
using System.Collections.Generic;

namespace SMProxy.Providers
{
    public interface IProxyListProvider
    {
        IEnumerable<Proxy> GetProxyList();
    }
}
