using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Definitions.Queries;

namespace ResourcePackerGUI.Application.Definitions.Handlers
{
    public class CreateDefinitionsQueryHandler : IRequestHandler<CreateDefinitionsQuery, IReadOnlyDictionary<string, string>>
    {
        private readonly ILogger<CreateDefinitionsQueryHandler> _logger;

        public CreateDefinitionsQueryHandler(ILogger<CreateDefinitionsQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<IReadOnlyDictionary<string, string>> Handle(CreateDefinitionsQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var definitions = CreateDefinitions(request);
                _logger.LogInformation("Created {definitionCount} definitions.", definitions.Count);
                return definitions;
            }, cancellationToken);
        }

        private IReadOnlyDictionary<string, string> CreateDefinitions(CreateDefinitionsQuery request)
        {
            var percentage = 0;
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            var processedItems = new Dictionary<string, string>();
            for (var i = 0; i < request.FilePaths.Count; i++)
            {
                var absolutePath = request.FilePaths[i];

                if (!File.Exists(absolutePath))
                {
                    _logger.LogWarning("File does not exist: {path}", absolutePath);
                    continue;
                }

                if (!GetRelativePath(request, absolutePath, out var relativePath))
                {
                    _logger.LogWarning("Could not create relative path from: {path}", absolutePath);
                    continue;
                }

                processedItems.Add(absolutePath, relativePath);
                percentage = (int)((double)(i + 1) / request.FilePaths.Count * 100);
            }

            return processedItems;
        }

        private static bool GetRelativePath(CreateDefinitionsQuery request, string absolutePath, out string relativePath)
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
    }
}
