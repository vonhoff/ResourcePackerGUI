using System.IO.Abstractions.TestingHelpers;
using ResourcePackerGUI.Application.Resources.Handlers;
using ResourcePackerGUI.Application.Resources.Queries;
using ResourcePackerGUI.Domain.Entities;

namespace Application.UnitTests.Resources
{
    public class GetConflictingResourcesQueryHandlerTests
    {
        private readonly IReadOnlyList<Resource> _resources = new List<Resource>()
        {
            new(Array.Empty<byte>(), default, name: "accept.png"),
            new(Array.Empty<byte>(), default, name: "deny.png"),
            new(Array.Empty<byte>(), default, name: "asterisk_orange.png"),
            new(Array.Empty<byte>(), default, name: "asterisk_blue.png"),
            new(Array.Empty<byte>(), default, name: "asterisk_red.png"),
            new(Array.Empty<byte>(), default, name: "award_star_gold_1.png"),
            new(Array.Empty<byte>(), default, name: "award_star_gold_2.png"),
            new(Array.Empty<byte>(), default, name: "award_star_gold_3.png"),
        };

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
            var sut = new GetConflictingResourcesQueryHandler(fileSystem);
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