using SM.Crawler.Libs.Crawling.ObjectMapping.Evaluators;
using SM.Libs.Utils;
using SMProxy.Providers;
using System;
using System.Collections.Generic;

namespace SM.Proxy.proxyscan.io.Provider
{
    public class ProxyScan_io : IProxyListProvider
    {
        internal class ArrayProxy
        {
            public IEnumerable<SMProxy.Providers.Proxy> Proxies { get; set; }
        }
        const string URL = "https://www.proxyscan.io/";
        public IEnumerable<SMProxy.Providers.Proxy> GetProxyList()
        {
            IMapper arrayProxyMapper = new Mapper(typeof(ArrayProxy), ".")
                .MapArray("Proxies", "//*[@id=\"loadPage\"]/tr", new Mapper(typeof(SMProxy.Providers.Proxy))
                .Map("Host", "./th[1]")
                .Map("Port", "./td[1]")
                .MapObject("Metadata", new Mapper(typeof(Metadata))
                    .Map("Country", "./td[2]")
                    .Map("Ping", "./td[3]/div[1]/div[1]")
                    .Map("Type", "./td[4]")
                    .Map("Anonymity", "./td[5]")
                 ));
            string html = DefaultHttpUtility.GetHtmlStringAsync(URL).Result;
            ArrayProxy arrayProxy = Evaluator.Evaluate(html, arrayProxyMapper) as ArrayProxy;
            return arrayProxy.Proxies;
        }
    }
}
