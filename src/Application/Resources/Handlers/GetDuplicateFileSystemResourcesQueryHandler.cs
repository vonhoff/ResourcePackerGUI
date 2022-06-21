using System.IO.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Resources.Queries;

namespace ResourcePackerGUI.Application.Resources.Handlers
{
    public class GetDuplicateFileSystemResourcesQueryHandler :
        IRequestHandler<GetDuplicateFileSystemResourcesQuery, IReadOnlyList<string>>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<GetDuplicateFileSystemResourcesQueryHandler> _logger;

        public GetDuplicateFileSystemResourcesQueryHandler(IFileSystem fileSystem,
            ILogger<GetDuplicateFileSystemResourcesQueryHandler> logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }

        public Task<IReadOnlyList<string>> Handle(GetDuplicateFileSystemResourcesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() => GetDuplicateFiles(request), cancellationToken);
        }

        private IReadOnlyList<string> GetDuplicateFiles(GetDuplicateFileSystemResourcesQuery request)
        {
            return (from path in request.PathEntries
                    where _fileSystem.File.Exists(path.AbsolutePath)
                    select path.AbsolutePath).ToList();
        }
    }
}