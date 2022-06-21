using MediatR;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Packaging.Queries
{
    public class BuildPackageQuery : IRequest
    {
        public BuildPackageQuery(IReadOnlyList<PathEntry> pathEntries, string output, string password = "")
        {
            PathEntries = pathEntries;
            Output = output;
            Password = password;
        }

        public string Output { get; init; }
        public string Password { get; init; }
        public IReadOnlyList<PathEntry> PathEntries { get; init; }
        public IProgress<int>? ProgressPrimary { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
        public IProgress<int>? ProgressSecondary { get; init; }
    }
}