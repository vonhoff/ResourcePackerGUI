using MediatR;
using ResourcePackerGUI.Application.Common.Interfaces;
using Serilog;

namespace ResourcePackerGUI.Application.Resources.Queries.UpdateResourceDefinitions
{
    public class UpdateResourceDefinitionsQueryHandler : IRequestHandler<UpdateResourceDefinitionsQuery, int>
    {
        private readonly IMediaTypeService _mediaTypeService;

        public UpdateResourceDefinitionsQueryHandler(IMediaTypeService mediaTypeService)
        {
            _mediaTypeService = mediaTypeService;
        }

        public Task<int> Handle(UpdateResourceDefinitionsQuery request, CancellationToken cancellationToken)
        {
            if (request.ChecksumDefinitions.Count == 0 || request.Resources.Count == 0)
            {
                throw new InvalidDataException("The provided data is invalid.");
            }

            var updated = 0;
            using (var progressTimer = new System.Timers.Timer(request.ProgressReportInterval))
            {
                var percentage = 0;

                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
                progressTimer.Enabled = request.Progress != null;

                for (var i = 0; i < request.Resources.Count; i++)
                {
                    percentage = (int)((double)(i + 1) / request.Resources.Count * 100);

                    var asset = request.Resources[i];
                    if (request.ChecksumDefinitions.TryGetValue(asset.Entry.Id, out var filePath))
                    {
                        // If the media type has not been found before,
                        // try to find the media type by its file extension.
                        asset.MediaType ??= _mediaTypeService.GetTypeByName(filePath);
                        asset.Name = filePath;
                        updated++;

                        Log.Debug("Updated asset: {@asset}",
                            new { asset.Entry.Id, asset.Name });

                        continue;
                    }

                    Log.Warning("Could not find definition for hash: {id}", asset.Entry.Id);
                }
            }

            if (updated == request.Resources.Count)
            {
                Log.Information("Updated all {count} resources.", request.Resources.Count);
            }
            else
            {
                Log.Warning("Updated {actual} out of {expected} resources.", updated, request.Resources.Count);
            }

            request.Progress?.Report(100);
            return Task.FromResult(updated);
        }
    }
}