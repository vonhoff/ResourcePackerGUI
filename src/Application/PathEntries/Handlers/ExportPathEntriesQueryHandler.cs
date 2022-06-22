using System.IO.Abstractions;
using System.Text;
using MediatR;
using ResourcePackerGUI.Application.PathEntries.Queries;

namespace ResourcePackerGUI.Application.PathEntries.Handlers
{
    public class ExportPathEntriesQueryHandler : IRequestHandler<ExportPathEntriesQuery>
    {
        private readonly IFileSystem _fileSystem;

        public ExportPathEntriesQueryHandler(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<Unit> Handle(ExportPathEntriesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var file = _fileSystem.FileStream.Create(request.Output, FileMode.Create);
                if (file == null)
                {
                    return Unit.Value;
                }

                var percentage = 0;
                using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
                progressTimer.Enabled = request.Progress != null;

                for (var i = 0; i < request.PathEntries.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var buffer = Encoding.UTF8.GetBytes(request.PathEntries[i].RelativePath);
                    file.Write(buffer);
                    percentage = (int)((double)(i + 1) / request.PathEntries.Count * 100);
                }

                file.Close();
                return Unit.Value;
            }, cancellationToken);
        }
    }
}
