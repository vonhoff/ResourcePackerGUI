using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class UpdateResourceDefinitionsQuery : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="UpdateResourceDefinitionsQuery"/> class.
        /// </summary>
        /// <param name="resources">The collection of resources to update.</param>
        /// <param name="checksumDefinitions">A read-only dictionary of checksum values and their original entries.</param>
        public UpdateResourceDefinitionsQuery(IReadOnlyList<Resource> resources, IReadOnlyDictionary<uint, string> checksumDefinitions)
        {
            Resources = resources;
            ChecksumDefinitions = checksumDefinitions;
        }

        /// <summary>
        /// The collection of resources to update according to the checksum dictionary.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// A read-only dictionary of checksum values and their original entries.
        /// </summary>
        public IReadOnlyDictionary<uint, string> ChecksumDefinitions { get; init; }
    }
}