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

namespace ResourcePackerGUI.Application.Resources.Queries.GetConflictingResources
{
    public class GetConflictingResourcesQueryHandler : IRequestHandler<GetConflictingResourcesQuery, IReadOnlyList<Resource>>
    {
        private readonly IFileSystem _fileSystem;

        public GetConflictingResourcesQueryHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<IReadOnlyList<Resource>> Handle(GetConflictingResourcesQuery request, CancellationToken cancellationToken)
        {
            var percentage = 0f;
            IReadOnlyList<Resource> list;
            using (var progressTimer = new System.Timers.Timer(request.ProgressReportInterval))
            {
                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
                progressTimer.Enabled = request.Progress != null;
                list = CollectFileConflicts(request, ref percentage);
            }

            Log.Information("{amount} conflicts found.", list.Count);
            request.Progress?.Report(100);
            return Task.FromResult(list);
        }

        /// <summary>
        /// Collects all conflicting files by checking for every file if it exists.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="percentage">A percentage to keep track of the amount of resources checked.</param>
        /// <returns>A read-only list of conflicting resources.</returns>
        private IReadOnlyList<Resource> CollectFileConflicts(GetConflictingResourcesQuery request, ref float percentage)
        {
            var list = new List<Resource>();
            for (var i = 0; i < request.Resources.Count; i++)
            {
                var resource = request.Resources[i];
                if (_fileSystem.File.Exists(Path.Join(request.BasePath, resource.Name)))
                {
                    list.Add(resource);
                }

                percentage = (i + 1f) / request.Resources.Count * 100f;
            }

            return list;
        }
    }
}