﻿using System.IO.Abstractions.TestingHelpers;
using Application.UnitTests.Common;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.PathEntries.Handlers;
using ResourcePackerGUI.Application.PathEntries.Queries;
using ResourcePackerGUI.Domain.Entities;
using Xunit;

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
            var query = new ExportPathEntriesQuery(_pathEntries, output);
            var sut = new ExportPathEntriesQueryHandler(fileSystem);
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
            var query = new ExportPathEntriesQuery(_pathEntries, string.Empty);
            var sut = new ExportPathEntriesQueryHandler(fileSystem);
            await Assert.ThrowsAsync<InvalidPathException>(() => sut.Handle(query, default));
        }
    }
}