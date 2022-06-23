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
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes("ForeSt\\maP.TmX\nsmW/smw_FoReGround.TSX   "));
            var query = new CreateChecksumDefinitionsQuery(stream);
            var sut = new CreateChecksumDefinitionsQueryHandler(_crc32Serivce, _logger);
            var result = await sut.Handle(query, default);
            Assert.True(result?.TryGetValue(3090263786, out _) == true);
            Assert.True(result?.TryGetValue(3106401826, out _) == true);
            Assert.True(result?.TryGetValue(3801234820, out _) == false);
        }
    }
}