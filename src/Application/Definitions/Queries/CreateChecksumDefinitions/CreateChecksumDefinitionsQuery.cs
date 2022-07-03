using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries.CreateChecksumDefinitions
{
    public record CreateChecksumDefinitionsQuery : IRequest<IReadOnlyDictionary<uint, string>>
    {
        /// <summary>
        /// Constructor for the <see cref="CreateChecksumDefinitionsQuery"/> class.
        /// </summary>
        /// <param name="fileStream">The stream of the specified definition file.</param>
        public CreateChecksumDefinitionsQuery(Stream fileStream)
        {
            FileStream = fileStream;
        }

        /// <summary>
        /// The stream of the specified definition file.
        /// </summary>
        public Stream FileStream { get; init; }

        /// <summary>
        /// An optional progress to keep track of the process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}