using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class UpdateResourceDefinitionsQuery : IRequest
    {
        public UpdateResourceDefinitionsQuery(IReadOnlyList<Resource> resources, IReadOnlyDictionary<uint, string> checksumDefinitions)
        {
            Resources = resources;
            ChecksumDefinitions = checksumDefinitions;
        }

        public IReadOnlyDictionary<uint, string> ChecksumDefinitions { get; init; }
        public IReadOnlyList<Resource> Resources { get; init; }
    }
}