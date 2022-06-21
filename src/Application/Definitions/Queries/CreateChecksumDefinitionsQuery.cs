using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries
{
    public class CreateChecksumDefinitionsQuery : IRequest<IReadOnlyDictionary<uint, string>>
    {
        public CreateChecksumDefinitionsQuery(Stream fileStream)
        {
            FileStream = fileStream;
        }

        public Stream FileStream { get; init; }
    }
}