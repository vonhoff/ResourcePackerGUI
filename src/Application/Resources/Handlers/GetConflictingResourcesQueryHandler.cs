using System.IO.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Resources.Queries;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class GetConflictingResourcesQueryHandler : IRequestHandler<GetConflictingResourcesQuery, IReadOnlyList<Resource>>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<GetConflictingResourcesQueryHandler> _logger;

        public GetConflictingResourcesQueryHandler(IFileSystem fileSystem, ILogger<GetConflictingResourcesQueryHandler> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public Task<IReadOnlyList<Resource>> Handle(GetConflictingResourcesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
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

                _logger.LogInformation("{amount} conflicts found.", list.Count);
                return list;
            }, cancellationToken);
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