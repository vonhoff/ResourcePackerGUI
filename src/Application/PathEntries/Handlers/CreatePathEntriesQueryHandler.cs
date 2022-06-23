using System.IO.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.PathEntries.Queries;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.PathEntries.Handlers
{
    public class CreatePathEntriesQueryHandler : IRequestHandler<CreatePathEntriesQuery, IReadOnlyList<PathEntry>>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<CreatePathEntriesQueryHandler> _logger;

        public CreatePathEntriesQueryHandler(IFileSystem fileSystem, ILogger<CreatePathEntriesQueryHandler> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public Task<IReadOnlyList<PathEntry>> Handle(CreatePathEntriesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var definitions = CreatePathEntries(request);
                _logger.LogInformation("Created {count} path entries.", definitions.Count);
                return definitions;
            }, cancellationToken);
        }

        /// <summary>
        /// Creates a read-only list of path entries.
        /// </summary>
        /// <param name="request">The request containing the progress instances and absolute file paths.</param>
        /// <returns>A read-only list of path entries.</returns>
        private IReadOnlyList<PathEntry> CreatePathEntries(CreatePathEntriesQuery request)
        {
            var percentage = 0;
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);

            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            var processedItems = new List<PathEntry>();
            for (var i = 0; i < request.AbsoluteFilePaths.Count; i++)
            {
                var absolutePath = request.AbsoluteFilePaths[i];

                if (!_fileSystem.File.Exists(absolutePath))
                {
                    _logger.LogWarning("File does not exist: {path}", absolutePath);
                    continue;
                }

                if (!CreateRelativePath(absolutePath, request.RelativeFilePathDepth, out var relativePath))
                {
                    _logger.LogWarning("Could not create relative path from: {path}", absolutePath);
                    continue;
                }

                processedItems.Add(new PathEntry(absolutePath, relativePath));
                percentage = (int)((double)(i + 1) / request.AbsoluteFilePaths.Count * 100);
            }

            return processedItems;
        }

        /// <summary>
        /// Creates a relative path from a provided absolute path and relative depth.
        /// </summary>
        /// <param name="absolutePath">The full path towards the file location.</param>
        /// <param name="relativeDepth">The depth of the relative path.</param>
        /// <param name="relativePath">The resulting relative path.</param>
        /// <returns><see langword="true"/> when successful, <see langword="false"/> otherwise.</returns>
        private static bool CreateRelativePath(string absolutePath, int relativeDepth, out string relativePath)
        {
            var pathNodes = absolutePath
                .Replace(@"\", "/")
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (pathNodes.Length < relativeDepth)
            {
                relativePath = string.Empty;
                return false;
            }

            relativePath = string.Join('/', pathNodes[relativeDepth..]);
            return true;
        }
    }
}