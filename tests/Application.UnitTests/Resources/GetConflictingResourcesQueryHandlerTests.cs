using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ResourcePackerGUI.Application.Resources.Handlers;
using ResourcePackerGUI.Application.Resources.Queries;
using ResourcePackerGUI.Domain.Entities;
using Xunit;

namespace Application.UnitTests.Resources
{
    public class GetConflictingResourcesQueryHandlerTests
    {
        private readonly ILogger<GetConflictingResourcesQueryHandler> _logger;

        private readonly IReadOnlyList<Resource> _resources = new List<Resource>()
        {
            new Resource(Array.Empty<byte>(), default, name: "accept.png"),
            new Resource(Array.Empty<byte>(), default, name: "deny.png"),
            new Resource(Array.Empty<byte>(), default, name: "asterisk_orange.png"),
            new Resource(Array.Empty<byte>(), default, name: "asterisk_blue.png"),
            new Resource(Array.Empty<byte>(), default, name: "asterisk_red.png"),
            new Resource(Array.Empty<byte>(), default, name: "award_star_gold_1.png"),
            new Resource(Array.Empty<byte>(), default, name: "award_star_gold_2.png"),
            new Resource(Array.Empty<byte>(), default, name: "award_star_gold_3.png"),
        };

        public GetConflictingResourcesQueryHandlerTests()
        {
            _logger = new NullLogger<GetConflictingResourcesQueryHandler>();
        }

        [Fact]
        public async Task GetConflictingResources()
        {
            var files = new Dictionary<string, MockFileData>
            {
                {"F:\\assets\\accept.png", new MockFileData(string.Empty)},
                {"F:\\assets\\deny.png", new MockFileData(string.Empty)},
                {"F:\\assets\\award_star_gold_1.png", new MockFileData(string.Empty)},
                {"F:\\assets\\award_star_gold_3.png", new MockFileData(string.Empty)}
            };

            const string basePath = "F:\\assets";
            var fileSystem = new MockFileSystem(files);
            var query = new GetConflictingResourcesQuery(basePath, _resources);
            var sut = new GetConflictingResourcesQueryHandler(fileSystem, _logger);
            var result = await sut.Handle(query, default);
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
            Assert.Equal("F:\\assets\\accept.png", Path.Join(basePath, result[0].Name));
            Assert.Equal("F:\\assets\\deny.png", Path.Join(basePath, result[1].Name));
            Assert.Equal("F:\\assets\\award_star_gold_1.png", Path.Join(basePath, result[2].Name));
            Assert.Equal("F:\\assets\\award_star_gold_3.png", Path.Join(basePath, result[3].Name));
        }
    }
}