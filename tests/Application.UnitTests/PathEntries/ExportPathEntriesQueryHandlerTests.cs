using System.IO.Abstractions.TestingHelpers;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.PathEntries.Commands.ExportPathEntries;
using ResourcePackerGUI.Domain.Entities;

namespace Application.UnitTests.PathEntries
{
    public class ExportPathEntriesQueryHandlerTests
    {
        private readonly List<PathEntry> _pathEntries = new()
        {
            new PathEntry("F:\\repos\\ResourcePacker\\Debug\\assets\\base.png", "assets/base.png"),
            new PathEntry("F:\\repos\\ResourcePacker\\Debug\\assets\\mushroom-red.png", "assets/mushroom-red.png")
        };

        [Fact]
        public async Task ExportPathEntries()
        {
            const string output = "F:\\repos\\ResourcePacker\\Debug\\assets\\export.txt";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(),
                "F:\\repos\\ResourcePacker\\Debug\\assets\\");
            var query = new ExportPathEntriesCommand(_pathEntries, output);
            var sut = new ExportPathEntriesCommandHandler(fileSystem);
            var expectedOutput =
                "assets/base.png" + Environment.NewLine +
                "assets/mushroom-red.png" + Environment.NewLine;

            await sut.Handle(query, default);
            Assert.Equal(expectedOutput, fileSystem.GetFile(output).TextContents);
        }

        [Fact]
        public async Task ExportPathEntries_NoOutput_ThrowsInvalidOutputException()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(),
                "F:\\repos\\ResourcePacker\\Debug\\assets\\");
            var query = new ExportPathEntriesCommand(_pathEntries, string.Empty);
            var sut = new ExportPathEntriesCommandHandler(fileSystem);
            await Assert.ThrowsAsync<InvalidPathException>(() => sut.Handle(query, default));
        }
    }
}