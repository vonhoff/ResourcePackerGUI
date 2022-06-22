using MediatR;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Packaging.Queries
{
    public class GetResourceListQuery : IRequest<IReadOnlyList<Resource>>
    {
        /// <summary>
        /// Constructor for the <see cref="GetResourceListQuery"/> class.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="binaryReader"></param>
        /// <param name="password"></param>
        public GetResourceListQuery(Package package, BinaryReader binaryReader, string password = "")
        {
            Package = package;
            BinaryReader = binaryReader;
            Password = password;
        }

        public Package Package { get; init; }
        public BinaryReader BinaryReader { get; init; }
        public string Password { get; init; }
        public IProgress<int>? ProgressPrimary { get; init; }
        public IProgress<int>? ProgressSecondary { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}