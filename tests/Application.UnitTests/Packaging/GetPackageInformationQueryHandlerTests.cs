using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.Packaging.Handlers;
using ResourcePackerGUI.Application.Packaging.Queries;
using ResourcePackerGUI.Domain.Structures;
using Xunit;

namespace Application.UnitTests.Packaging
{
    [Collection(QueryCollection.CollectionName)]
    public class GetPackageInformationQueryHandlerTests
    {
        private const ulong PackHeaderId = 30227092120757586;

        private static readonly byte[] SampleResPack =
        {
            82, 101, 115, 80, 97, 99, 107, 0, 0, 0, 0, 0, 2, 0, 0, 0, 126, 51, 41, 241,
            130, 137, 209, 247, 5, 0, 0, 0, 5, 0, 0, 0, 56, 0, 0, 0, 174, 73, 137, 182,
            222, 157, 40, 118, 6, 0, 0, 0, 6, 0, 0, 0, 61, 0, 0, 0, 72, 101, 108, 108,
            111, 87, 111, 114, 108, 100, 33
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

        private readonly ILogger<GetPackageInformationQueryHandler> _logger;

        public GetPackageInformationQueryHandlerTests()
        {
            _logger = new NullLogger<GetPackageInformationQueryHandler>();
        }

        [Fact]
        public async Task GetPackageInformation_OnValidPackage()
        {
            await using var stream = new MemoryStream(SampleResPack);
            using var binaryReader = new BinaryReader(stream);
            var query = new GetPackageInformationQuery(binaryReader);
            var sut = new GetPackageInformationQueryHandler(_logger);
            var result = await sut.Handle(query, default);
            Assert.NotNull(result);
            Assert.True(result.Header.Id == PackHeaderId);
            Assert.True(result.Header.NumberOfEntries == 2);
            Assert.True(result.Entries.Count == 2);
            Assert.True(result.Entries[0].Equals(ExpectedFirstEntry));
            Assert.True(result.Entries[1].Equals(ExpectedSecondEntry));
        }

        [Fact]
        public async Task GetPackageInformation_OnInvalidPackage_ThrowsInvalidHeaderException()
        {
            await using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Great tools help make great games."));
            using var binaryReader = new BinaryReader(stream);
            var query = new GetPackageInformationQuery(binaryReader);
            var sut = new GetPackageInformationQueryHandler(_logger);
            await Assert.ThrowsAsync<InvalidHeaderException>(() => sut.Handle(query, default));
        }
    }
}
