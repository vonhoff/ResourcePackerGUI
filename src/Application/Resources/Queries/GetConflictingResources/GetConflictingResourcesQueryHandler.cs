using System.IO.Abstractions;
using MediatR;
using ResourcePackerGUI.Domain.Entities;
using Serilog;

namespace ResourcePackerGUI.Application.Resources.Queries.GetConflictingResources
{
    public class GetConflictingResourcesQueryHandler : IRequestHandler<GetConflictingResourcesQuery, IReadOnlyList<Resource>>
    {
        private readonly IFileSystem _fileSystem;

        public GetConflictingResourcesQueryHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<IReadOnlyList<Resource>> Handle(GetConflictingResourcesQuery request, CancellationToken cancellationToken)
        {
            var percentage = 0;
            IReadOnlyList<Resource> list;
            using (var progressTimer = new System.Timers.Timer(request.ProgressReportInterval))
            {
                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
                progressTimer.Enabled = request.Progress != null;
                list = CollectFileConflicts(request, ref percentage);
            }

            Log.Information("{amount} conflicts found.", list.Count);
            request.Progress?.Report(100);
            return Task.FromResult(list);
        }

        /// <summary>
        /// Collects all conflicting files by checking for every file if it exists.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="percentage">A percentage to keep track of the amount of resources checked.</param>
        /// <returns>A read-only list of conflicting resources.</returns>
        private IReadOnlyList<Resource> CollectFileConflicts(GetConflictingResourcesQuery request, ref int percentage)
        {
            var list = new List<Resource>();
            for (var i = 0; i < request.Resources.Count; i++)
            {
                var resource = request.Resources[i];
                if (_fileSystem.File.Exists(Path.Join(request.BasePath, resource.Name)))
                {
                    list.Add(resource);
                }

                percentage = (int)((double)(i + 1) / request.Resources.Count * 100);
            }

            return list;
        }
    }
}