using MediatR;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Packaging.Queries
{
    public class GetResourceListQuery : IRequest<IReadOnlyList<Resource>>
    {
        public GetResourceListQuery(Package package, BinaryReader binaryReader, string password = "")
        {
            Package = package;
            BinaryReader = binaryReader;
            Password = password;
        }

        public BinaryReader BinaryReader { get; init; }
        public Package Package { get; init; }
        public string Password { get; init; }
        public IProgress<int>? ProgressPrimary { get; init; }
        public IProgress<int>? ProgressSecondary { get; init; }
        public int ProgressReportInterval { get; init; } = 100;
    }
}