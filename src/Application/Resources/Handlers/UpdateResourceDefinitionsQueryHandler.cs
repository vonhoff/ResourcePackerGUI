using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Resources.Queries;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class UpdateResourceDefinitionsQueryHandler : IRequestHandler<UpdateResourceDefinitionsQuery>
    {
        private readonly IMediaTypeService _mediaTypeService;
        private readonly ILogger<UpdateResourceDefinitionsQueryHandler> _logger;

        public UpdateResourceDefinitionsQueryHandler(IMediaTypeService mediaTypeService,
            ILogger<UpdateResourceDefinitionsQueryHandler> logger)
        {
            _mediaTypeService = mediaTypeService;
            _logger = logger;
        }

        public Task<Unit> Handle(UpdateResourceDefinitionsQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (request.ChecksumDefinitions.Count == 0 || request.Resources.Count == 0)
                {
                    _logger.LogError("Could not update assets with definitions: {@info}",
                        new
                        {
                            ResourcesCount = request.Resources.Count,
                            DefinitionCount = request.ChecksumDefinitions.Count
                        });
                    return Unit.Value;
                }

                foreach (var asset in request.Resources)
                {
                    if (request.ChecksumDefinitions.TryGetValue(asset.Entry.Id, out var filePath))
                    {
                        // If the media type has not been found before,
                        // try to find the media type by the file extension.
                        asset.MediaType ??= _mediaTypeService.GetTypeByName(asset.Name);
                        asset.Name = filePath;
                    }
                    else
                    {
                        _logger.LogWarning("Could not find definition for hash: {id}", asset.Entry.Id);
                        continue;
                    }

                    _logger.LogDebug("Updated asset: {@asset}",
                        new { asset.Entry.Id, asset.Name });
                }

                return Unit.Value;
            }, cancellationToken);
        }
    }
}