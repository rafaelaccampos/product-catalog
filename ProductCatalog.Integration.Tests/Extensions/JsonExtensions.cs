using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mime;
using System.Text;

namespace ProductCatalog.Integration.Tests.Extensions
{
    public static class JsonExtensions
    {
        private static void RemoveIdFromJToken(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                var obj = (JObject)token;
                obj.Remove("id");
                obj.Remove("Id");
                foreach (var property in obj.Properties())
                {
                    RemoveIdFromJToken(property.Value);
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                var array = (JArray)token;
                foreach(var item in array)
                {
                    RemoveIdFromJToken(item);
                }
            }
        }
        public static HttpContent ToJsonContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, mediaType: MediaTypeNames.Application.Json);
        }

        public static void ShouldBeAnEquivalentJson(this string actual, string expected)
        {
            var actualJToken = actual.ToJToken();
            var expectedJToken = expected.ToJToken();

            RemoveIdFromJToken(actualJToken);
            RemoveIdFromJToken(expectedJToken);

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
