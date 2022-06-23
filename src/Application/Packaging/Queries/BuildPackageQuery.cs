using MediatR;
using ResourcePackerGUI.Application.Common.Models;

namespace ResourcePackerGUI.Application.Packaging.Queries
{
    public class BuildPackageQuery : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="BuildPackageQuery"/> class.
        /// </summary>
        /// <param name="pathEntries">A read-only collection of <see cref="PathEntry"/> instances.</param>
        /// <param name="output">The destination of the package file.</param>
        /// <param name="password">An optional password used to encrypt the data located at the path entries.</param>
        public BuildPackageQuery(IReadOnlyList<PathEntry> pathEntries, string output, string password = "")
        {
            PathEntries = pathEntries;
            Output = output;
            Password = password;
        }

        /// <summary>
        /// The destination of the package file.
        /// </summary>
        public string Output { get; init; }

        /// <summary>
        /// The password used to encrypt the data located at the path entries.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// A read-only collection of <see cref="PathEntry"/> instances.
        /// </summary>
        public IReadOnlyList<PathEntry> PathEntries { get; init; }

        /// <summary>
        /// An optional secondary progress instance to keep track of the encryption progress.
        /// </summary>
        public IProgress<int>? ProgressSecondary { get; init; }

        /// <summary>
        /// An optional primary progress instance to keep track of the amount of files processed.
        /// </summary>
        public IProgress<int>? ProgressPrimary { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}