using System.Text;
using Application.UnitTests.Common.Fixture;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.Packaging.Queries.GetPackageInformation;
using ResourcePackerGUI.Domain.Structures;

namespace Application.UnitTests.Packaging
{
    [Collection(QueryCollection.CollectionName)]
    public class GetPackageInformationQueryHandlerTests
    {
        [Fact]
        public async Task GetPackageInformation_OnInvalidPackage_ThrowsInvalidHeaderException()
        {
            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Great tools help make great games."));
            using var binaryReader = new BinaryReader(stream);
            var query = new GetPackageInformationQuery(binaryReader);
            var sut = new GetPackageInformationQueryHandler();
            await Assert.ThrowsAsync<InvalidHeaderException>(() => sut.Handle(query, default));
        }

        [Fact]
        public async Task GetPackageInformation_OnValidPackage()
        {
            await using var stream = new MemoryStream(SampleResourcePackage);
            using var binaryReader = new BinaryReader(stream);
            var query = new GetPackageInformationQuery(binaryReader);
            var sut = new GetPackageInformationQueryHandler();
            var result = await sut.Handle(query, default);
            Assert.NotNull(result);
            Assert.Equal(ExpectedPackageHeader, result.Header);
            Assert.Equal(2, result.Entries.Count);
            Assert.Equal(ExpectedFirstEntry, result.Entries[0]);
            Assert.Equal(ExpectedSecondEntry, result.Entries[1]);
        }

        #region Sample resource package

        /// <summary>
        /// A resource package created with the original command-line tool.
        /// </summary>
        private static readonly byte[] SampleResourcePackage =
        {
            82, 101, 115, 80, 97, 99, 107, 0, 0, 0, 0, 0, 2, 0, 0, 0, 126, 51, 41, 241,
            130, 137, 209, 247, 5, 0, 0, 0, 5, 0, 0, 0, 56, 0, 0, 0, 174, 73, 137, 182,
            222, 157, 40, 118, 6, 0, 0, 0, 6, 0, 0, 0, 61, 0, 0, 0, 72, 101, 108, 108,
            111, 87, 111, 114, 108, 100, 33
        };

        #endregion Sample resource package

        #region Expected entities

        private static readonly PackageHeader ExpectedPackageHeader = new()
        {
            Id = 30227092120757586,
            Reserved = 0,
            NumberOfEntries = 2
        };

        private static readonly Entry ExpectedFirstEntry = new()
        {
            Crc = 4157704578,
            DataSize = 5,
            Id = 4046009214,
            Offset = 56,
            PackSize = 5
        };

        private static readonly Entry ExpectedSecondEntry = new()
        {
            Crc = 1982373342,
            DataSize = 6,
            Id = 3062450606,
            Offset = 61,
            PackSize = 6
        };

        #endregion Expected entities
    }
}