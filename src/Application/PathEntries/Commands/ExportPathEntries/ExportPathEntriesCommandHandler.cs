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
using System.Text;
using MediatR;
using ResourcePackerGUI.Application.Common.Exceptions;

namespace ResourcePackerGUI.Application.PathEntries.Commands.ExportPathEntries
{
    public class ExportPathEntriesCommandHandler : IRequestHandler<ExportPathEntriesCommand>
    {
        private readonly IFileSystem _fileSystem;

        public ExportPathEntriesCommandHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<Unit> Handle(ExportPathEntriesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Output) ||
                request.Output.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new InvalidPathException("The specified output is invalid.", request.Output);
            }

            var file = _fileSystem.FileStream.Create(request.Output, FileMode.Create);
            if (file == null)
            {
                throw new InvalidDataException("The file stream is null.");
            }

            var percentage = 0;
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            for (var i = 0; i < request.PathEntries.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var buffer = Encoding.UTF8.GetBytes(request.PathEntries[i].RelativePath + Environment.NewLine);
                file.Write(buffer);
                percentage = (int)((double)(i + 1) / request.PathEntries.Count * 100);
            }

            file.Close();
            return Task.FromResult(Unit.Value);
        }
    }
}