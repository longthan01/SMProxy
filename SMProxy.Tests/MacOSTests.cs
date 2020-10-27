using NUnit.Framework;
using SMProxy.Abstracts;
using SMProxy.Providers;

namespace SMProxy.Tests
{
    public class MacOSTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateProxySetterForMacOS_ShouldPass()
        {
            IProxySetter ps = new MacOSProxySetter();
            ps.Set("1.1.1.1", "8080");
            Assert.True(true);
        }
    }
}