using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries.UpdateResourceDefinitions
{
    public class UpdateResourceDefinitionsQuery : IRequest<int>
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

        /// <summary>
        /// An optional progress instance to keep track of the process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}