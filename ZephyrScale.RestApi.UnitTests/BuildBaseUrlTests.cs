using NUnit.Framework;
using ZephyrScale.RestApi.Service;

namespace ZephyrScale.RestApi.UnitTests
{
    [TestFixture]
    public class BuildBaseUrlTests
    {
        private class TestableServiceBase : ZephyrScaleServiceBase
        {
            public static string Invoke(string url) => BuildBaseUrl(url);
        }

        [TestCase("https://api.zephyrscale.smartbear.com", "https://api.zephyrscale.smartbear.com:443")]
        [TestCase("https://api.zephyrscale.smartbear.com:443", "https://api.zephyrscale.smartbear.com:443")]
        [TestCase("https://api.zephyrscale.smartbear.com:8080", "https://api.zephyrscale.smartbear.com:8080")]
        [TestCase("http://myserver.com", "http://myserver.com:80")]
        [TestCase("http://myserver.com:80", "http://myserver.com:80")]
        [TestCase("http://myserver.com:8080", "http://myserver.com:8080")]
        [TestCase("https://myserver.com/some/path", "https://myserver.com:443")]
        [TestCase("myserver.com", "https://myserver.com:443")]
        [TestCase("myserver.com:8080", "https://myserver.com:8080")]
        public void BuildBaseUrl_ReturnsExpected(string input, string expected)
        {
            Assert.That(TestableServiceBase.Invoke(input), Is.EqualTo(expected));
        }
    }
}
