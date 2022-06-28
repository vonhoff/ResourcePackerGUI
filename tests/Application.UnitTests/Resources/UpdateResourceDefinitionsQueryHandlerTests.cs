using Application.UnitTests.Common.Fixture;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Resources.Handlers;
using ResourcePackerGUI.Application.Resources.Queries;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.Structures;

namespace Application.UnitTests.Resources
{
    [Collection(QueryCollection.CollectionName)]
    public class UpdateResourceDefinitionsQueryHandlerTests
    {
        private readonly IMediaTypeService _mediaTypeService;

        public UpdateResourceDefinitionsQueryHandlerTests(QueryTestFixture fixture)
        {
            _mediaTypeService = fixture.MediaTypeService;
        }

        [Fact]
        public async Task UpdateResourceDefinitions()
        {
            var resources = new List<Resource>
            {
                new (Array.Empty<byte>(), new Entry { Id = 74397442 }),
                new (Array.Empty<byte>(), new Entry { Id = 1763913621 }),
                new (Array.Empty<byte>(), new Entry { Id = 3741099771 })
            };

            var checksumDefinitions = new Dictionary<uint, string>
            {
                {74397442, "accept.png"},
                {1763913621, "asterisk_orange.png"},
                {3741099771, "award_star_gold_3.png"}
            };

            var query = new UpdateResourceDefinitionsQuery(resources, checksumDefinitions);
            var sut = new UpdateResourceDefinitionsQueryHandler(_mediaTypeService);
            await sut.Handle(query, default);
            Assert.Equal("accept.png", resources[0].Name);
            Assert.Equal("asterisk_orange.png", resources[1].Name);
            Assert.Equal("award_star_gold_3.png", resources[2].Name);
        }
    }
}