using System.IO.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Enums;
using ResourcePackerGUI.Application.Resources.Queries;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class ExportResourcesQueryHandler : IRequestHandler<ExportResourcesQuery>
    {
        private const string NumberPattern = " ({0})";
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<ExportResourcesQueryHandler> _logger;

        public ExportResourcesQueryHandler(IFileSystem fileSystem, ILogger<ExportResourcesQueryHandler> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public Task<Unit> Handle(ExportResourcesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var basePath = request.BasePath;

                if (!Path.EndsInDirectorySeparator(basePath))
                {
                    basePath += Path.DirectorySeparatorChar;
                }

                using (var progressTimer = new System.Timers.Timer(request.ProgressReportInterval))
                {
                    var percentage = 0;

                    // ReSharper disable once AccessToModifiedClosure
                    progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
                    progressTimer.Enabled = request.Progress != null;

                    for (var i = 0; i < request.Resources.Count; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        percentage = (int)((double)(i + 1) / request.Resources.Count * 100);

                        var resource = request.Resources[i];
                        if (!CreateFileInfo(basePath, resource, out var fileInfo))
                        {
                            continue;
                        }

                        if (fileInfo.Exists)
                        {
                            var conflictResolveActions = request.ConflictResolveActions;
                            if (!TryResolvingFileConflict(conflictResolveActions, resource, ref fileInfo))
                            {
                                continue;
                            }
                        }

                        ExportFile(fileInfo, resource);
                    }
                }

                request.Progress?.Report(100);
                return Unit.Value;
            }, cancellationToken);
        }

        /// <summary>
        /// Exports the resource to the specified file in <paramref name="fileInfo"/>.
        /// </summary>
        /// <param name="fileInfo">The information instance containing all output information.</param>
        /// <param name="resource">The resource to be extracted.</param>
        private void ExportFile(IFileInfo fileInfo, Resource resource)
        {
            try
            {
                fileInfo.Directory?.Create();

                var file = _fileSystem.File.OpenWrite(fileInfo.FullName);
                using var binaryWriter = new BinaryWriter(file);
                binaryWriter.Write(resource.Data);
                binaryWriter.Flush();
                binaryWriter.Close();
                _logger.LogDebug("Extracted {name} to: {path}",
                    Path.GetFileName(resource.Name), fileInfo.FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not extract asset: {path}", fileInfo.FullName);
            }
        }

        /// <summary>
        /// Attempts to create file information by combining the base path with the resource name.
        /// </summary>
        /// <param name="basePath">The base path to use </param>
        /// <param name="resource">The resource containing the file name.</param>
        /// <param name="fileInfo">The resulting file information.</param>
        /// <returns><see langword="true"/> when succeeded, <see langword="false"/> otherwise.</returns>
        private bool CreateFileInfo(string basePath, Resource resource, out IFileInfo fileInfo)
        {
            try
            {
                fileInfo = _fileSystem.FileInfo.FromFileName(basePath + resource.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create file information from: {path}", basePath + resource.Name);
                fileInfo = _fileSystem.FileInfo.FromFileName(string.Empty);
                return false;
            }
        }

        /// <summary>
        /// Attempts to resolve the file conflict by finding the corresponding resolve method.
        /// </summary>
        /// <param name="conflictResolveActions">A dictionary of resolve methods.</param>
        /// <param name="resource">The resource for looking up the resolve method.</param>
        /// <param name="fileInfo">The file information to update after resolving.</param>
        /// <returns><see langword="true"/> when resolved, <see langword="false"/> otherwise.</returns>
        private bool TryResolvingFileConflict(IReadOnlyDictionary<Resource, FileConflictResolveMethod>? conflictResolveActions,
            Resource resource, ref IFileInfo fileInfo)
        {
            var path = fileInfo.FullName;
            if (conflictResolveActions == null || !conflictResolveActions.TryGetValue(resource, out var resolveAction))
            {
                _logger.LogWarning("Ignored, resolve action not defined for: {path}", path);
                return false;
            }

            switch (resolveAction)
            {
                case FileConflictResolveMethod.None:
                {
                    _logger.LogInformation("Ignored: {path}", path);
                    return false;
                }
                case FileConflictResolveMethod.Replace:
                {
                    _logger.LogInformation("Replacing: {path}", path);
                    break;
                }
                case FileConflictResolveMethod.KeepBoth:
                {
                    path = Path.HasExtension(path) ?
                        GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern)) :
                        GetNextFilename(path + NumberPattern);
                    fileInfo = _fileSystem.FileInfo.FromFileName(path);
                    break;
                }
                default:
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Retrieves the next available file name by adding a number pattern to the provided file.
        /// </summary>
        /// <param name="pattern">The path with an index place-holder for looking up an available file.</param>
        /// <returns>An available file path.</returns>
        /// <exception cref="ArgumentException">When the pattern did not include an index place-holder.</exception>
        private string GetNextFilename(string pattern)
        {
            var tmp = string.Format(pattern, 1);
            if (tmp == pattern)
            {
                throw new ArgumentException("The pattern must include an index place-holder", nameof(pattern));
            }

            if (!_fileSystem.File.Exists(tmp))
            {
                return tmp;
            }

            int min = 1, max = 2;

            while (_fileSystem.File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                var pivot = (max + min) / 2;
                if (_fileSystem.File.Exists(string.Format(pattern, pivot)))
                {
                    min = pivot;
                }
                else
                {
                    max = pivot;
                }
            }

            return string.Format(pattern, max);
        }
    }
}