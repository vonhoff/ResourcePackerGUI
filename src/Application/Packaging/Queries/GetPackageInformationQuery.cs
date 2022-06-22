using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Packaging.Queries
{
    public class GetPackageInformationQuery : IRequest<Package>
    {
        /// <summary>
        /// Constructor for the <see cref="GetPackageInformationQuery"/> class.
        /// </summary>
        /// <param name="binaryReader">The binary reader to retrieve the package information.</param>
        public GetPackageInformationQuery(BinaryReader binaryReader)
        {
            BinaryReader = binaryReader;
        }

        /// <summary>
        /// The binary reader to retrieve the package information.
        /// </summary>
        public BinaryReader BinaryReader { get; init; }

        /// <summary>
        /// A progress instance to keep track of the amount of entries being loaded.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instance when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}