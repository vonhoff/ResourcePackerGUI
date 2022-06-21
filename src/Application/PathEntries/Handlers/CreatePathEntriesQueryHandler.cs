using System.IO.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.PathEntries.Queries;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.PathEntries.Handlers
{
    public class CreatePathEntriesQueryHandler : IRequestHandler<CreatePathEntriesQuery, IReadOnlySet<PathEntry>>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<CreatePathEntriesQueryHandler> _logger;

        public CreatePathEntriesQueryHandler(IFileSystem fileSystem, ILogger<CreatePathEntriesQueryHandler> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public Task<IReadOnlySet<PathEntry>> Handle(CreatePathEntriesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var definitions = CreateRelativePathSet(request);
                _logger.LogInformation("Created {definitionCount} definitions.", definitions.Count);
                return definitions;
            }, cancellationToken);
        }

        private static bool GetRelativePath(CreatePathEntriesQuery request, string absolutePath, out string relativePath)
        {
            var pathNodes = absolutePath
                .Replace(@"\", "/")
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (pathNodes.Length < request.RelativeFilePathDepth)
            {
                relativePath = string.Empty;
                return false;
            }

            relativePath = string.Join('/', pathNodes[request.RelativeFilePathDepth..]).ToLowerInvariant();
            return true;
        }

        private IReadOnlySet<PathEntry> CreateRelativePathSet(CreatePathEntriesQuery request)
        {
            var percentage = 0;
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            var processedItems = new HashSet<PathEntry>(0);
            for (var i = 0; i < request.FilePaths.Count; i++)
            {
                var absolutePath = request.FilePaths[i];

                if (!_fileSystem.File.Exists(absolutePath))
                {
                    _logger.LogWarning("File does not exist: {path}", absolutePath);
                    continue;
                }

                if (!GetRelativePath(request, absolutePath, out var relativePath))
                {
                    _logger.LogWarning("Could not create relative path from: {path}", absolutePath);
                    continue;
                }

                processedItems.Add(new PathEntry(absolutePath, relativePath));
                percentage = (int)((double)(i + 1) / request.FilePaths.Count * 100);
            }

            return processedItems;
        }
    }
}
