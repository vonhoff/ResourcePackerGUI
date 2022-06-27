using Application.UnitTests.Common.Mocks;
using ResourcePackerGUI.Application.Common.Interfaces;

namespace Application.UnitTests.Common.Fixture
{
    public class QueryTestFixture
    {
        public QueryTestFixture()
        {
            AesEncryptionService = new MockAesEncryptionService();
            Crc32Service = new MockCrc32Service();
            MediaTypeService = new MockMediaTypeService();
        }

        public IAesEncryptionService AesEncryptionService { get; }
        public ICrc32Service Crc32Service { get; }
        public IMediaTypeService MediaTypeService { get; }
    }
}