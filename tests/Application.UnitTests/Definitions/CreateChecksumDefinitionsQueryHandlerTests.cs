using System.Text;
using Application.UnitTests.Common.Fixture;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Definitions.Queries.CreateChecksumDefinitions;

namespace Application.UnitTests.Definitions
{
    [Collection(QueryCollection.CollectionName)]
    public class CreateChecksumDefinitionsQueryHandlerTests
    {
        private readonly ICrc32Service _crc32Service;

        public CreateChecksumDefinitionsQueryHandlerTests(QueryTestFixture fixture)
        {
            _crc32Service = fixture.Crc32Service;
        }

        [Fact]
        public async Task CreateChecksumDefinitions()
        {
            await using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes("forest/map.tmx\nsmw/smw_foreground.tsx"));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Service);
            var result = await sut.Handle(query, default);
            Assert.True(result.Count == 2);
            Assert.True(result.TryGetValue(3090263786, out _));
            Assert.True(result.TryGetValue(3106401826, out _));
        }

        [Fact]
        public async Task CreateChecksumDefinitions_ShouldSkipDuplicates()
        {
            await using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes("sonic/base.tmx\nsonic/base.tmx\nsonic/base.tmx"));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Service);
            var result = await sut.Handle(query, default);
            Assert.True(result.Count == 1);
        }

        [Fact]
        public async Task CreateChecksumDefinitions_ShouldSkipBlankLines()
        {
            await using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes("   \n\nforest/map.tmx\n   \nsmw/smw_foreground.tsx\n \n    \n"));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Service);
            var result = await sut.Handle(query, default);
            Assert.True(result.Count == 2);
            Assert.True(result.TryGetValue(3090263786, out _));
            Assert.True(result.TryGetValue(3106401826, out _));
        }

        [Fact]
        public async Task CreateChecksumDefinitions_NameNormalization()
        {
            await using var stream = new MemoryStream(
                Encoding.UTF8.GetBytes("Tf4\\heLLaRm.TxT \n  RacEr/trEEs.pNg "));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Service);
            var result = await sut.Handle(query, default);
            Assert.True(result.Count == 2);
            Assert.True(result.TryGetValue(3411133111, out _));
            Assert.True(result.TryGetValue(859863405, out _));
        }
    }
}