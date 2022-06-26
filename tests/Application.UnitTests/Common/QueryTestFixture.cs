using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Infrastructure.Services;

namespace Application.UnitTests.Common
{
    public class QueryTestFixture
    {
        public QueryTestFixture()
        {
            AesEncryptionService = new AesEncryptionService();
            Crc32Service = new Crc32Service();
            MediaTypeService = new MediaTypeService();
        }

        public IAesEncryptionService AesEncryptionService { get; }
        public ICrc32Service Crc32Service { get; }
        public IMediaTypeService MediaTypeService { get; }
    }
}