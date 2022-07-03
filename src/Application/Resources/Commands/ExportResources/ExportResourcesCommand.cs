using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Commands.ExportResources
{
    public record ExportResourcesCommand : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="ExportResourcesCommand"/> class.
        /// </summary>
        /// <param name="basePath">The root folder to write the resources to.</param>
        /// <param name="resources">The resources to export.</param>
        /// <param name="conflictingNameReplacements">A dictionary of resolve methods for conflicting file encounters.</param>
        public ExportResourcesCommand(string basePath, IReadOnlyList<Resource> resources,
            IReadOnlyDictionary<Resource, string>? conflictingNameReplacements = null)
        {
            Resources = resources;
            ConflictingNameReplacements = conflictingNameReplacements;
            BasePath = basePath;
        }

        /// <summary>
        /// A dictionary of file names for conflicting encounters.
        /// </summary>
        public IReadOnlyDictionary<Resource, string>? ConflictingNameReplacements { get; init; }

        /// <summary>
        /// The resources to export.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// The root folder to write the resources to.
        /// </summary>
        public string BasePath { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the export process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}