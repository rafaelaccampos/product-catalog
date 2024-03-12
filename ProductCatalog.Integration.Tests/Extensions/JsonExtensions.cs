using FluentAssertions;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ProductCatalog.Integration.Tests.Extensions
{
    public static class JsonExtensions
    {
        public static HttpContent ToJsonContent(this object obj)
        {
            var json = JsonSerializer.Serialize(obj);
            return new StringContent(json, Encoding.UTF8, mediaType: MediaTypeNames.Application.Json);
        }

        //public static void ShouldBeAnEquivalentJson(this string actual, string expected)
        //{
        //    var actualJToken = actual.ToJToken();
        //    var expectedJToken = expected.ToJToken();
        //    var areEquals = JToken.DeepEquals(actualJToken, expectedJToken);
        //    areEquals.Should().BeTrue();
        //}
    }
}
