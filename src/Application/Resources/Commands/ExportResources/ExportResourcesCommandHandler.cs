#region GNU General Public License

/* Copyright 2022 Simon Vonhoff & Contributors
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using System.IO.Abstractions;
using MediatR;
using ResourcePackerGUI.Domain.Entities;
using Serilog;

namespace ResourcePackerGUI.Application.Resources.Commands.ExportResources
{
    public class ExportResourcesCommandHandler : IRequestHandler<ExportResourcesCommand>
    {
        private readonly IFileSystem _fileSystem;

        public ExportResourcesCommandHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<Unit> Handle(ExportResourcesCommand request, CancellationToken cancellationToken)
        {
            var basePath = request.BasePath;

            if (!Path.EndsInDirectorySeparator(basePath))
            {
                basePath += Path.DirectorySeparatorChar;
            }

            using (var progressTimer = new System.Timers.Timer(request.ProgressReportInterval))
            {
                var percentage = 0f;

                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
                progressTimer.Enabled = request.Progress != null;

                for (var i = 0; i < request.Resources.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    percentage = (i + 1f) / request.Resources.Count * 100f;

                    var resource = request.Resources[i];
                    if (!CreateFileInfo(basePath, resource, out var fileInfo))
                    {
                        continue;
                    }

                    if (!fileInfo.Exists ||
                        TryResolvingFileConflict(request.ConflictingNameReplacements, resource, ref fileInfo))
                    {
                        ExportFile(fileInfo, resource);
                    }
                }
            }

            request.Progress?.Report(100f);
            return Task.FromResult(Unit.Value);
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
                Log.Debug("Extracted {name} to: {path}",
                    Path.GetFileName(resource.Name), fileInfo.FullName);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Could not extract resource: {path}", fileInfo.FullName);
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