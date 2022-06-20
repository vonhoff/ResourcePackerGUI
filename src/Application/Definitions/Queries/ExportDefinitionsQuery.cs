using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries
{
    public class ExportDefinitionsQuery : IRequest
    {
        public ExportDefinitionsQuery(IReadOnlyList<string> filePaths, string output)
        {
            FilePaths = filePaths;
            Output = output;
        }

        public IReadOnlyList<string> FilePaths { get; init; }
        public string Output { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}