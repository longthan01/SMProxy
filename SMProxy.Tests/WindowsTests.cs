using NUnit.Framework;
using SMProxy.Abstracts;
using SMProxy.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMProxy.Tests
{
    public class WindowsTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateProxySetterForWindows_ShouldPass()
        {
            IProxySetter ps = new WindowsProxySetter(null);
            ps.Set("1.1.1.3", "8080");
            Assert.True(true);
        }
    }
}
