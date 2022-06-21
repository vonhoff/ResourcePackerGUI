using MediatR;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.PathEntries.Queries
{
    public class ExportPathEntriesQuery : IRequest
    {
        public ExportPathEntriesQuery(IReadOnlyList<PathEntry> pathEntries, string output)
        {
            PathEntries = pathEntries;
            Output = output;
        }

        public IReadOnlyList<PathEntry> PathEntries { get; init; }
        public string Output { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}