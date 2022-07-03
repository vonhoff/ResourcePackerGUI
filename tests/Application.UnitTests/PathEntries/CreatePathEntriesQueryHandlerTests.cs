using ResourcePackerGUI.Application.PathEntries.Queries.CreatePathEntries;

namespace Application.UnitTests.PathEntries
{
    public class CreatePathEntriesQueryHandlerTests
    {
        [Fact]
        public async Task CreatePathEntries()
        {
            List<string> filePaths = new()
            {
                "F:\\repos\\ResourcePacker\\Debug\\assets\\base.png",
                "F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png",
            };

            var query = new CreatePathEntriesQuery(filePaths, 4);
            var sut = new CreatePathEntriesQueryHandler();
            var result = await sut.Handle(query, default);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("assets/base.png", result[0].RelativePath);
            Assert.Equal("assets/mushroom-red.png", result[1].RelativePath);
            Assert.Equal("F:\\repos\\ResourcePacker\\Debug\\assets\\base.png", result[0].AbsolutePath);
            Assert.Equal("F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png", result[1].AbsolutePath);
        }

        [Fact]
        public async Task CreatePathEntries_ShouldIgnoreInvalidEntries()
        {
            IReadOnlyList<string> absoluteFilePaths = new List<string>()
            {
                string.Empty,
                " ",
                "\r\n",
                ":{}$%=;&",
                "F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png",
                "F:\\repos\\ResourcePacker\\mushroom-blue.png",
                "F:\\repos\\assets\\base.png",
                "F:\\repos\\assets\\section.png",
                "F:\\repos\\ResourcePacker\\player.png",
                "F:\\repos\\ResourcePacker\\Debug\\assets\\base.png"
            };

            var query = new CreatePathEntriesQuery(absoluteFilePaths, 4);
            var sut = new CreatePathEntriesQueryHandler();
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