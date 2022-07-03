﻿using System.IO.Abstractions;
using MediatR;

using ResourcePackerGUI.Application.Resources.Queries;
using ResourcePackerGUI.Domain.Entities;
using Serilog;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class ExportResourcesQueryHandler : IRequestHandler<ExportResourcesQuery, int>
    {
        private readonly IFileSystem _fileSystem;

        public ExportResourcesQueryHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<int> Handle(ExportResourcesQuery request, CancellationToken cancellationToken)
        {
            var basePath = request.BasePath;
            var exported = 0;

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

                    if (fileInfo.Exists &&
                        !TryResolvingFileConflict(request.ConflictingNameReplacements, resource, ref fileInfo))
                    {
                        continue;
                    }

                    if (ExportFile(fileInfo, resource))
                    {
                        exported++;
                    }
                }
            }

            request.Progress?.Report(100);
            return Task.FromResult(exported);
        }

        /// <summary>
        /// Exports the resource to the specified file in <paramref name="fileInfo"/>.
        /// </summary>
        /// <param name="fileInfo">The information instance containing all output information.</param>
        /// <param name="resource">The resource to be extracted.</param>
        private bool ExportFile(IFileInfo fileInfo, Resource resource)
        {
            try
            {
                fileInfo.Directory?.Create();

                var file = _fileSystem.File.OpenWrite(fileInfo.FullName);
                using var binaryWriter = new BinaryWriter(file);
                binaryWriter.Write(resource.Data);
                binaryWriter.Flush();
                binaryWriter.Close();
                Log.Debug("Extracted {name} to: {path}",
                    Path.GetFileName(resource.Name), fileInfo.FullName);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not extract asset: {path}", fileInfo.FullName);
                return false;
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
                Log.Error(ex, "Could not create file information from: {path}", basePath + resource.Name);
                fileInfo = _fileSystem.FileInfo.FromFileName(string.Empty);
                return false;
            }
        }

        /// <summary>
        /// Attempts to resolve the file conflict by finding the corresponding resolve method.
        /// </summary>
        /// <param name="conflictingNameReplacements">A dictionary of conflict name replacements.</param>
        /// <param name="resource">The resource for looking up the resolve method.</param>
        /// <param name="fileInfo">The file information to update after resolving.</param>
        /// <returns><see langword="true"/> when resolved, <see langword="false"/> otherwise.</returns>
        private bool TryResolvingFileConflict(IReadOnlyDictionary<Resource, string>? conflictingNameReplacements,
            Resource resource, ref IFileInfo fileInfo)
        {
            var path = fileInfo.FullName;
            if (conflictingNameReplacements == null || !conflictingNameReplacements.TryGetValue(resource, out var replacement))
            {
                Log.Debug("Ignored: {path}", path);
                return false;
            }

            fileInfo = _fileSystem.FileInfo.FromFileName(replacement);
            return true;
        }
    }
}