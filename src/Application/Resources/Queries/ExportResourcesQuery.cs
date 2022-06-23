﻿using MediatR;
using ResourcePackerGUI.Application.Common.Enums;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Resources.Queries
{
    public class ExportResourcesQuery : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="ExportResourcesQuery"/> class.
        /// </summary>
        /// <param name="basePath">The root folder to write the resources to.</param>
        /// <param name="resources">The resources to export.</param>
        /// <param name="conflictResolveActions">A dictionary of resolve methods for conflicting file encounters.</param>
        public ExportResourcesQuery(string basePath, IReadOnlyList<Resource> resources,
            IReadOnlyDictionary<Resource, FileConflictResolveMethod>? conflictResolveActions)
        {
            Resources = resources;
            ConflictResolveActions = conflictResolveActions;
            BasePath = basePath;
        }

        /// <summary>
        /// A dictionary of resolve methods for conflicting file encounters.
        /// </summary>
        public IReadOnlyDictionary<Resource, FileConflictResolveMethod>? ConflictResolveActions { get; init; }

        /// <summary>
        /// The resources to export.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// The root folder to write the resources to.
        /// </summary>
        public string BasePath { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the export process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}