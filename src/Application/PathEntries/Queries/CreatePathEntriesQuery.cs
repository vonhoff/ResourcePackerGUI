using MediatR;
using ResourcePackerGUI.Application.Common.Models;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.PathEntries.Queries
{
    public class CreatePathEntriesQuery : IRequest<IReadOnlySet<PathEntry>>
    {
        public CreatePathEntriesQuery(IReadOnlyList<string> filePaths, int relativeFilePathDepth)
        {
            FilePaths = filePaths;
            RelativeFilePathDepth = relativeFilePathDepth;
        }

        public IReadOnlyList<string> FilePaths { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
        public int RelativeFilePathDepth { get; init; }
    }
}