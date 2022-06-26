using System.IO.Abstractions.TestingHelpers;
using System.Text;
using Application.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ResourcePackerGUI.Application.PathEntries.Handlers;
using ResourcePackerGUI.Application.PathEntries.Queries;
using Xunit;

namespace Application.UnitTests.PathEntries
{

    public class CreatePathEntriesQueryHandlerTests
    {
        private readonly ILogger<CreatePathEntriesQueryHandler> _logger;

        public CreatePathEntriesQueryHandlerTests()
        {
            _logger = new NullLogger<CreatePathEntriesQueryHandler>();
        }

        [Fact]
        public async Task CreatePathEntries_ValidFiles()
        {
            Dictionary<string, MockFileData> mockFiles = new()
            {
                { "F:\\repos\\ResourcePacker\\Debug\\assets\\base.png", new MockFileData(Array.Empty<byte>()) },
                { "F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png", new MockFileData(Array.Empty<byte>()) },
            };

            var fileSystem = new MockFileSystem(mockFiles, "F:\\repos\\ResourcePacker\\Debug\\");
            var query = new CreatePathEntriesQuery(mockFiles.Keys.ToList(), 4);
            var sut = new CreatePathEntriesQueryHandler(fileSystem, _logger);
            var result = await sut.Handle(query, default);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("assets/base.png", result[0].RelativePath);
            Assert.Equal("assets/mushroom-red.png", result[1].RelativePath);
            Assert.Equal("F:\\repos\\ResourcePacker\\Debug\\assets\\base.png", result[0].AbsolutePath);
            Assert.Equal("F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png", result[1].AbsolutePath);
        }

        [Fact]
        public async Task CreatePathEntries_ShouldIgnoreInvalidFilePaths()
        {
            Dictionary<string, MockFileData> mockFiles = new()
            {
                { "F:\\repos\\assets\\base.png", new MockFileData(Array.Empty<byte>()) },
                { "F:\\repos\\assets\\section.png", new MockFileData(Array.Empty<byte>()) },
                { "F:\\repos\\ResourcePacker\\Debug\\assets\\base.png", new MockFileData(Array.Empty<byte>()) },
                { "F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png", new MockFileData(Array.Empty<byte>()) },
            };

            IReadOnlyList<string> absoluteFilePaths = new List<string>()
            {
                string.Empty,
                " ",
                "\r\n",
                ":{}$%=;&",
                "F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png",
                "F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-blue.png",
                "F:\\repos\\assets\\base.png",
                "F:\\repos\\assets\\section.png",
                "F:\\repos\\ResourcePacker\\assets\\player.png",
                "F:\\repos\\ResourcePacker\\Debug\\assets\\base.png"
            };

            var fileSystem = new MockFileSystem(mockFiles, "F:\\repos\\ResourcePacker\\Debug\\");
            var query = new CreatePathEntriesQuery(absoluteFilePaths, 4);
            var sut = new CreatePathEntriesQueryHandler(fileSystem, _logger);
            var result = await sut.Handle(query, default);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("assets/mushroom-red.png", result[0].RelativePath);
            Assert.Equal("assets/base.png", result[1].RelativePath);
            Assert.Equal("F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png", result[0].AbsolutePath);
            Assert.Equal("F:\\repos\\ResourcePacker\\Debug\\assets\\base.png", result[1].AbsolutePath);
        }
    }
}