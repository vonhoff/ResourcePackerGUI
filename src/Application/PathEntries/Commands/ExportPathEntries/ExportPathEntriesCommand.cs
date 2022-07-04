﻿using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.PathEntries.Commands.ExportPathEntries
{
    public record ExportPathEntriesCommand : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="ExportPathEntriesCommand"/> class.
        /// </summary>
        /// <param name="pathEntries">The path entries to export.</param>
        /// <param name="output">The output file to write the entries to.</param>
        public ExportPathEntriesCommand(IReadOnlyList<PathEntry> pathEntries, string output)
        {
            PathEntries = pathEntries;
            Output = output;
        }

        /// <summary>
        /// The path entries to export.
        /// </summary>
        public IReadOnlyList<PathEntry> PathEntries { get; init; }

        /// <summary>
        /// The output file location to write to.
        /// </summary>
        public string Output { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the writing process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}