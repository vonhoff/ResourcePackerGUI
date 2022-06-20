using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries
{
    public class CreateDefinitionsQuery : IRequest<IReadOnlyDictionary<string, string>>
    {
        public CreateDefinitionsQuery(IReadOnlyList<string> filePaths, int relativeFilePathDepth)
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