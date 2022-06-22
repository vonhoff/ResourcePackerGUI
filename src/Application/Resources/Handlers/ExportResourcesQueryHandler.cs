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
                        percentage = (int)((double)(i + 1) / request.Resources.Count * 100);
                        cancellationToken.ThrowIfCancellationRequested();

                        var resource = request.Resources[i];
                        if (!GetFileInfo(basePath, resource, out var fileInfo))
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
        /// <param name="fileInfo">The information instance containing all file information.</param>
        /// <param name="resource"></param>
        private void ExportFile(IFileInfo fileInfo, Resource resource)
        {
            fileInfo.Directory?.Create();

            try
            {
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

        private bool GetFileInfo(string basePath, Resource resource, out IFileInfo fileInfo)
        {
            try
            {
                fileInfo = _fileSystem.FileInfo.FromFileName(basePath + resource.Name);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get file information from: {path}", basePath + resource.Name);
                fileInfo = _fileSystem.FileInfo.FromFileName(string.Empty);
                return false;
            }
        }

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

        private bool TryResolvingFileConflict(IReadOnlyDictionary<Resource, FileConflictResolveMethod>? conflictResolveActions, Resource resource,
                    ref IFileInfo fileInfo)
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
    }
}