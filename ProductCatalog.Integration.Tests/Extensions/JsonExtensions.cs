using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Text;

namespace ProductCatalog.Integration.Tests.Extensions
{
    public static class JsonExtensions
    {
        public static HttpContent ToJsonContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, mediaType: MediaTypeNames.Application.Json);
        }

        public static void ShouldBeAnEquivalentJson(this string actual, string expected)
        {
            var actualJToken = actual.ToJToken();
            var expectedJToken = expected.ToJToken();

            var areEquals = JToken.DeepEquals(actualJToken, expectedJToken);
            areEquals.Should().BeTrue();
        }

        private static JToken ToJToken(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The JSON sent to the JToken() method is empty or is null.");
            }

            return JToken.Parse(text);
        }
    }
}
