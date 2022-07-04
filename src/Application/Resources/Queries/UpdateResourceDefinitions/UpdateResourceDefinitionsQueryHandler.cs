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

                    var resource = request.Resources[i];
                    if (resource.NameDefined)
                    {
                        updated++;
                        continue;
                    }

                    if (request.ChecksumDefinitions.TryGetValue(resource.Entry.Id, out var filePath))
                    {
                        // If the media type has not been found before,
                        // try to find the media type by its file extension.
                        resource.MediaType ??= _mediaTypeService.GetTypeByName(filePath);
                        resource.Name = filePath;
                        updated++;

                        Log.Debug("Updated resource: {@asset}",
                            new { resource.Entry.Id, resource.Name });

                        continue;
                    }

                    Log.Warning("Could not find definition for hash: {id}", resource.Entry.Id);
                }
            }

            if (updated == request.Resources.Count)
            {
                Log.Information("All {count} resource names are defined.", request.Resources.Count);
            }
            else
            {
                Log.Warning("{actual} out of {expected} resource names are defined.", updated, request.Resources.Count);
            }

            request.Progress?.Report(100);
            return Task.FromResult(updated);
        }
    }
}