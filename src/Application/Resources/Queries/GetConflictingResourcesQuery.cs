using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class GetConflictingResourcesQuery : IRequest<IReadOnlyList<Resource>>
    {
        public GetConflictingResourcesQuery(IReadOnlyList<Resource> resources)
        {
            Resources = resources;
        }

        public IReadOnlyList<Resource> Resources { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}