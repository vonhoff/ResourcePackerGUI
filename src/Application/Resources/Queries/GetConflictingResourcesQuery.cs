using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class GetConflictingResourcesQuery : IRequest<IReadOnlyList<Resource>>
    {
        /// <summary>
        /// Constructor for the <see cref="GetConflictingResourcesQuery"/> class.
        /// </summary>
        /// <param name="basePath">The base folder to check for conflicts.</param>
        /// <param name="resources">The collection of resources.</param>
        public GetConflictingResourcesQuery(string basePath, IReadOnlyList<Resource> resources)
        {
            BasePath = basePath;
            Resources = resources;
        }

        /// <summary>
        /// The root folder to check for conflicts.
        /// </summary>
        public string BasePath { get; init; }

        /// <summary>
        /// The collection of resources to check if a file
        /// with the same name already exists on the file system.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the checking process.
        /// </summary>
        public IProgress<double>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}