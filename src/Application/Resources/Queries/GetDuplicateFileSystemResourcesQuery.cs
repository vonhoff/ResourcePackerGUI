using MediatR;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class GetDuplicateFileSystemResourcesQuery : IRequest<IReadOnlyList<string>>
    {
        public GetDuplicateFileSystemResourcesQuery(IReadOnlyList<PathEntry> pathEntries)
        {
            PathEntries = pathEntries;
        }

        public IReadOnlyList<PathEntry> PathEntries { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}