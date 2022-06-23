using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Infrastructure.Services;
using Xunit;

namespace Application.UnitTests.Common
{
    [CollectionDefinition(CollectionName)]
    public class QueryCollection : ICollectionFixture<QueryTestFixture>
    {
        public const string CollectionName = "QueryCollection";
    }

    public class QueryTestFixture
    {
        public QueryTestFixture()
        {
            AesEncryptionService = new AesEncryptionService();
            Crc32Service = new Crc32Service();
            MediaTypeService = new MediaTypeService();
            LoggerFactory = new LoggerFactory();
        }

        public LoggerFactory LoggerFactory { get; }
        public IAesEncryptionService AesEncryptionService { get; }
        public ICrc32Service Crc32Service { get; }
        public IMediaTypeService MediaTypeService { get; }
    }
}