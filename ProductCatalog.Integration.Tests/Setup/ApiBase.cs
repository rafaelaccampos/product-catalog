namespace ProductCatalog.Integration.Tests.Setup
{
    public class ApiBase : DatabaseBase
    {
        protected HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = TestEnvironment.Factory.CreateClient();
        }
    }
}
