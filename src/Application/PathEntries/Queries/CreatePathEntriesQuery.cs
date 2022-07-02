using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.PathEntries.Queries
{
    public class CreatePathEntriesQuery : IRequest<IReadOnlyList<PathEntry>>
    {
        /// <summary>
        /// Constructor for the <see cref="CreatePathEntriesQuery"/> class.
        /// </summary>
        /// <param name="absoluteFilePaths">A list of absolute file paths to create relative paths from.</param>
        /// <param name="relativeFilePathDepth">The relative file path depth.</param>
        public CreatePathEntriesQuery(IReadOnlyList<string> absoluteFilePaths, int relativeFilePathDepth)
        {
            AbsoluteFilePaths = absoluteFilePaths;
            RelativeFilePathDepth = relativeFilePathDepth;
        }

        /// <summary>
        /// A list of absolute file paths to create relative paths from.
        /// </summary>
        public IReadOnlyList<string> AbsoluteFilePaths { get; init; }

        /// <summary>
        /// The relative file path depth.
        /// </summary>
        public int RelativeFilePathDepth { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the path creation process.
        /// </summary>
        public IProgress<double>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}