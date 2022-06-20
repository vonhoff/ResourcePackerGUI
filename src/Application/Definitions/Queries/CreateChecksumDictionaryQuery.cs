using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries
{
    public class CreateChecksumDictionaryQuery : IRequest<IReadOnlyDictionary<uint, string>>
    {
        public CreateChecksumDictionaryQuery(Stream fileStream)
        {
            FileStream = fileStream;
        }

        public Stream FileStream { get; init; }
    }
}