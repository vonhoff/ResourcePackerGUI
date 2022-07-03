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