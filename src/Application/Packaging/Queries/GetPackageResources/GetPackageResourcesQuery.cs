using MediatR;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.Structures;

namespace ResourcePackerGUI.Application.Packaging.Queries.GetPackageResources
{
    public record GetPackageResourcesQuery : IRequest<IReadOnlyList<Resource>>
    {
        /// <summary>
        /// Constructor for the <see cref="GetPackageResourcesQuery"/> class.
        /// </summary>
        /// <param name="entries">The collection of entries for retrieving all resources.</param>
        /// <param name="binaryReader">The binary reader for the specified package file.</param>
        /// <param name="password">The password for use to decrypt the assets.</param>
        public GetPackageResourcesQuery(IReadOnlyList<Entry> entries, BinaryReader binaryReader, string password = "")
        {
            Entries = entries;
            BinaryReader = binaryReader;
            Password = password;
        }

        /// <summary>
        /// The collection of entries for retrieving all resources.
        /// </summary>
        public IReadOnlyList<Entry> Entries { get; init; }

        /// <summary>
        /// The binary reader for the specified package file.
        /// </summary>
        public BinaryReader BinaryReader { get; init; }

        /// <summary>
        /// The password for use to decrypt the assets.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// An optional progress to keep track of the amount of resources loaded.
        /// </summary>
        public IProgress<int>? ProgressPrimary { get; init; }

        /// <summary>
        /// An optional progress to keep track of the decryption process.
        /// </summary>
        public IProgress<int>? ProgressSecondary { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}