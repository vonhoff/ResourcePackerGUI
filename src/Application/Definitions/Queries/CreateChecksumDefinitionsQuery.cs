using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries
{
    public class CreateChecksumDefinitionsQuery : IRequest<IReadOnlyDictionary<uint, string>>
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
    }
}