using Xunit;

namespace Worker.Tests
{
    [CollectionDefinition("Tests collection")]
    public class TestsCollection: ICollectionFixture<CustomApplicationFactory<Startup>>
    {
    }
}
