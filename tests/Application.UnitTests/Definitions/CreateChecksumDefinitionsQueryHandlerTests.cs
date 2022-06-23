using System.Text;
using Application.UnitTests.Common;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Definitions.Handlers;
using ResourcePackerGUI.Application.Definitions.Queries;
using Xunit;

namespace Application.UnitTests.Definitions
{
    [Collection(QueryCollection.CollectionName)]
    public class CreateChecksumDefinitionsQueryHandlerTests
    {
        private readonly ICrc32Service _crc32Serivce;
        private readonly ILogger<CreateChecksumDefinitionsQueryHandler> _logger;

        public CreateChecksumDefinitionsQueryHandlerTests(QueryTestFixture fixture)
        {
            _crc32Serivce = fixture.Crc32Service;
            _logger = new Logger<CreateChecksumDefinitionsQueryHandler>(fixture.LoggerFactory);
        }

        [Fact]
        public async Task CreateChecksumDefinitions()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("forest/map.tmx\nsmw/smw_foreground.tsx"));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Serivce, _logger);
            var result = await sut.Handle(query, default);
            Assert.True(result?.TryGetValue(3090263786, out _) == true);
            Assert.True(result?.TryGetValue(3106401826, out _) == true);
        }

        [Fact]
        public async Task CreateChecksumDefinitions_ShouldSkipDuplicates()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("sonic/base.tmx\nsonic/base.tmx\nsonic/base.tmx"));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Serivce, _logger);
            var result = await sut.Handle(query, default);
            Assert.True(result?.Count == 1);
        }

        [Fact]
        public async Task CreateChecksumDefinitions_NameNormalization()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("  \n Tf4\\heLLarm.TxT \n\n  RacEr/trEEs.pNg\n  \n"));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Serivce, _logger);
            var result = await sut.Handle(query, default);
            Assert.True(result?.Count == 2);
            Assert.True(result?.TryGetValue(3411133111, out _) == true);
            Assert.True(result?.TryGetValue(859863405, out _) == true);
        }
    }
}