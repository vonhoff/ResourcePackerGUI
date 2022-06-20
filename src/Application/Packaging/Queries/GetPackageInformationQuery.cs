using MediatR;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Packaging.Queries
{
    public class GetPackageInformationQuery : IRequest<Package>
    {
        public GetPackageInformationQuery(BinaryReader binaryReader)
        {
            BinaryReader = binaryReader;
        }

        public BinaryReader BinaryReader { get; init; }
        public IProgress<int>? Progress { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}