using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.Structures;

namespace Domain.UnitTests.Entities
{
    public class ResourceEntityTests
    {
        [Fact]
        public void CreateNameByMimeType()
        {
            var sample1 = new Resource(Array.Empty<byte>(),
                    new Entry { Id = 12345678 },
                    new MediaType { Extensions = new[] { "png" } });

            var sample2 = new Resource(Array.Empty<byte>(),
                new Entry { Id = 87654321 },
                new MediaType { Extensions = new[] { "txt", "text", "log" } });

            Assert.Equal("12345678.png", sample1.Name);
            Assert.Equal("87654321.txt", sample2.Name);
        }

        [Fact]
        public void GetNameWhenDefined()
        {
            var sample1 = new Resource(Array.Empty<byte>(),
                new Entry { Id = 12345678 },
                new MediaType { Extensions = new[] { "png" } },
                "Entry1.png");

            var sample2 = new Resource(Array.Empty<byte>(),
                new Entry { Id = 87654321 },
                new MediaType { Extensions = new[] { "txt", "json", "exe" } },
                "Entry2.png");

            Assert.Equal("Entry1.png", sample1.Name);
            Assert.Equal("Entry2.png", sample2.Name);
        }
    }
}