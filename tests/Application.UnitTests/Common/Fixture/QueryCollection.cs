using Xunit;

namespace Application.UnitTests.Common.Fixture
{
    [CollectionDefinition(CollectionName)]
    public class QueryCollection : ICollectionFixture<QueryTestFixture>
    {
        public const string CollectionName = "QueryCollection";
    }
}