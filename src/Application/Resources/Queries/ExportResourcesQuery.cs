using MediatR;
using ResourcePackerGUI.Application.Common.Enums;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class ExportResourcesQuery : IRequest
    {
        public ExportResourcesQuery(string basePath, IReadOnlyList<Resource> resources,
            IReadOnlyDictionary<Resource, FileConflictResolveMethod>? conflictResolveActions)
        {
            Resources = resources;
            ConflictResolveActions = conflictResolveActions;
            BasePath = basePath;
        }

        public IReadOnlyDictionary<Resource, FileConflictResolveMethod>? ConflictResolveActions { get; init; }
        public IReadOnlyList<Resource> Resources { get; init; }
        public string BasePath { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}