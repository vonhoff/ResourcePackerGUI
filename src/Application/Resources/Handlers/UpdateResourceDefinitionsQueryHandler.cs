using MediatR;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Resources.Queries;
using Serilog;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class UpdateResourceDefinitionsQueryHandler : IRequestHandler<UpdateResourceDefinitionsQuery>
    {
        private readonly IMediaTypeService _mediaTypeService;

        public UpdateResourceDefinitionsQueryHandler(IMediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

        public Task<Unit> Handle(UpdateResourceDefinitionsQuery request, CancellationToken cancellationToken)
        {
            if (request.ChecksumDefinitions.Count == 0 || request.Resources.Count == 0)
            {
                throw new InvalidDataException("The provided data is invalid.");
            }

            foreach (var asset in request.Resources)
            {
                if (request.ChecksumDefinitions.TryGetValue(asset.Entry.Id, out var filePath))
                {
                    // If the media type has not been found before,
                    // try to find the media type by its file extension.
                    asset.MediaType ??= _mediaTypeService.GetTypeByName(filePath);
                    asset.Name = filePath;
                }
                else
                {
                    Log.Warning("Could not find definition for hash: {id}", asset.Entry.Id);
                    continue;
                }

                Log.Debug("Updated asset: {@asset}",
                    new { asset.Entry.Id, asset.Name });
            }

            return Task.FromResult(Unit.Value);
        }
    }
}