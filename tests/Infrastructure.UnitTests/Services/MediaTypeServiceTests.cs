using System.Text;
using ResourcePackerGUI.Infrastructure.Services;

namespace Infrastructure.UnitTests.Services
{
    public class MediaTypeServiceTests
    {
        private readonly MediaTypeService _mediaTypeService;

        public MediaTypeServiceTests()
        {
            _mediaTypeService = new MediaTypeService();
        }

        [Fact]
        public void XML_FromData()
        {
            var mime = _mediaTypeService.GetTypeByData(XmlSample);
            Assert.Equal("text/xml", mime?.Name);
        }

        [Fact]
        public void XML_FromName()
        {
            var mime = _mediaTypeService.GetTypeByName("file.xml");
            Assert.Equal("text/xml", mime?.Name);
        }

        [Fact]
        public void JSON_FromData()
        {
            var mime = _mediaTypeService.GetTypeByData(JsonSample);
            Assert.Equal("application/json", mime?.Name);
        }

        [Fact]
        public void JSON_FromName()
        {
            var mime = _mediaTypeService.GetTypeByName("script.json");
            Assert.Equal("application/json", mime?.Name);
        }

        [Fact]
        public void GIF_FromData()
        {
            var mime = _mediaTypeService.GetTypeByData(GifSample);
            Assert.Equal("image/gif", mime?.Name);
        }

        [Fact]
        public void GIF_FromName()
        {
            var mime = _mediaTypeService.GetTypeByName("animation.gif");
            Assert.Equal("image/gif", mime?.Name);
        }

        [Fact]
        public void PNG_FromData()
        {
            var mime = _mediaTypeService.GetTypeByData(PngSample);
            Assert.Equal("image/png", mime?.Name);
        }

        [Fact]
        public void PNG_FromName()
        {
            var mime = _mediaTypeService.GetTypeByName("image.png");
            Assert.Equal("image/png", mime?.Name);
        }

        [Fact]
        public void JPEG_FromData()
        {
            var mime = _mediaTypeService.GetTypeByData(JpgSample);
            Assert.Equal("image/jpeg", mime?.Name);
        }

        [Fact]
        public void JPEG_FromName()
        {
            var mime = _mediaTypeService.GetTypeByName("target.jpg");
            Assert.Equal("image/jpeg", mime?.Name);
        }

        #region Samples

        private static readonly byte[] XmlSample = Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<breakfast_menu>\r\n  <food>\r\n    <name>Belgian Waffles</name>\r\n    <price>$5.95</price>\r\n    <description>\r\n      two of our famous Belgian Waffles with plenty of real maple syrup\r\n    </description>\r\n    <calories>650</calories>\r\n  </food>\r\n  <food>\r\n    <name>Strawberry Belgian Waffles</name>\r\n    <price>$7.95</price>\r\n    <description>\r\n      Belgian waffles covered with an assortment of fresh berries and whipped cream\r\n    </description>\r\n    <calories>900</calories>\r\n  </food>\r\n</breakfast_menu>");

        private static readonly byte[] JsonSample = Encoding.UTF8.GetBytes("{\r\n  \"name\": \"John Doe\",\r\n  \"age\": 25,\r\n  \"city\": \"New York\"\r\n}");

        private static readonly byte[] GifSample =
        {
            71, 73, 70, 56, 57, 97, 100, 0, 22, 0, 242, 2, 0, 0, 153, 153, 0, 0, 0, 102,
            204, 255, 102, 255, 255, 51, 102, 102, 0, 255, 255, 255, 255, 255, 0, 0, 0,
            33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0, 33,
            249, 4, 4, 30, 0, 255, 0, 44, 0, 0, 0, 0, 100, 0, 22, 0, 0, 3, 228, 40, 50,
            220, 254, 48, 202, 73, 171, 189, 80, 41, 204, 187, 255, 28, 161, 109, 96,
            105, 158, 20, 33, 142, 64, 235, 190, 112, 44, 207, 116, 109, 223, 56, 160,
            174, 90, 238, 255, 192, 224, 109, 55, 18, 8, 103, 133, 100, 242, 40, 91, 50,
            105, 68, 214, 211, 149, 12, 88, 157, 83, 64, 53, 27, 139, 246, 184, 219,
            150, 85, 171, 164, 150, 149, 133, 22, 90, 93, 8, 96, 185, 94, 5, 151, 252,
            174, 186, 211, 233, 171, 210, 122, 215, 211, 201, 115, 58, 42, 69, 129, 104,
            105, 100, 1, 136, 0, 119, 104, 137, 138, 139, 125, 125, 115, 113, 70, 129,
            140, 120, 134, 137, 85, 141, 108, 142, 157, 143, 89, 148, 129, 46, 125, 118,
            124, 118, 156, 160, 138, 97, 161, 131, 82, 89, 88, 155, 109, 128, 91, 169,
            181, 152, 154, 179, 112, 174, 95, 96, 141, 185, 118, 116, 169, 194, 122,
            185, 158, 83, 162, 150, 124, 158, 203, 164, 203, 99, 206, 208, 210, 199, 79,
            201, 163, 215, 216, 46, 214, 217, 220, 147, 188, 114, 221, 225, 173, 60, 10,
            59, 230, 231, 232, 233, 234, 235, 236, 237, 238, 239, 233, 35, 240, 243,
            244, 245, 246, 244, 26, 247, 250, 251, 252, 243, 229, 253, 0, 3, 6, 20, 144,
            0, 0, 33, 249, 4, 5, 30, 0, 2, 0, 44, 30, 0, 5, 0, 39, 0, 9, 0, 0, 2, 57,
            132, 143, 169, 203, 25, 255, 154, 100, 241, 84, 112, 167, 206, 217, 66, 31,
            65, 129, 33, 146, 99, 248, 121, 38, 6, 142, 43, 118, 134, 102, 90, 93, 162,
            237, 170, 108, 11, 151, 112, 159, 235, 212, 20, 40, 135, 203, 247, 67, 12,
            113, 201, 34, 80, 231, 12, 20, 0, 0, 59
        };

        private static readonly byte[] PngSample = {
            137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 128,
            0, 0, 0, 16, 4, 3, 0, 0, 0, 44, 118, 106, 187, 0, 0, 0, 48, 80, 76, 84, 69,
            0, 136, 238, 0, 29, 132, 0, 32, 137, 1, 40, 154, 0, 27, 128, 1, 47, 166, 1,
            56, 186, 1, 49, 172, 0, 38, 148, 1, 55, 182, 0, 35, 143, 1, 44, 161, 1, 43,
            158, 1, 53, 178, 1, 52, 176, 0, 34, 141, 61, 118, 69, 151, 0, 0, 0, 249, 73,
            68, 65, 84, 56, 203, 149, 211, 177, 13, 194, 48, 16, 133, 97, 86, 200, 8,
            185, 21, 220, 48, 0, 43, 156, 82, 208, 179, 1, 242, 0, 52, 22, 27, 100, 133,
            244, 84, 89, 193, 43, 120, 5, 23, 94, 0, 222, 225, 7, 138, 18, 132, 225, 75,
            82, 222, 175, 147, 98, 239, 14, 212, 209, 157, 28, 205, 228, 105, 164, 29,
            109, 2, 211, 29, 15, 2, 23, 250, 35, 32, 53, 128, 121, 179, 220, 64, 117,
            214, 118, 64, 170, 137, 28, 41, 220, 110, 218, 14, 72, 95, 173, 3, 71, 176,
            68, 51, 208, 55, 55, 200, 57, 227, 107, 110, 16, 166, 48, 65, 88, 110, 96,
            13, 12, 67, 13, 156, 240, 108, 3, 20, 200, 145, 26, 108, 144, 233, 68, 239,
            64, 247, 250, 7, 210, 139, 185, 210, 34, 0, 95, 2, 7, 20, 186, 15, 27, 68,
            227, 98, 84, 74, 25, 210, 167, 13, 186, 122, 134, 132, 174, 20, 73, 135,
            161, 6, 82, 202, 246, 110, 3, 44, 72, 85, 2, 69, 194, 236, 217, 36, 218, 6,
            72, 74, 41, 82, 68, 214, 1, 44, 224, 189, 122, 109, 6, 48, 43, 40, 172, 54,
            152, 227, 115, 250, 151, 128, 224, 2, 217, 6, 46, 24, 23, 102, 192, 124,
            244, 195, 235, 4, 166, 52, 54, 3, 248, 246, 206, 142, 192, 126, 113, 141,
            135, 167, 17, 5, 36, 198, 117, 224, 1, 119, 108, 147, 212, 68, 91, 165, 163,
            0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
        };

        private static readonly byte[] JpgSample =
        {
            255, 216, 255, 224, 0, 16, 74, 70, 73, 70, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0,
            255, 219, 0, 132, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 6, 6, 5, 6, 6, 8, 7, 7,
            7, 7, 8, 12, 9, 9, 9, 9, 9, 12, 19, 12, 14, 12, 12, 14, 12, 19, 17, 20, 16,
            15, 16, 20, 17, 30, 23, 21, 21, 23, 30, 34, 29, 27, 29, 34, 42, 37, 37, 42,
            52, 50, 52, 68, 68, 92, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 6, 6, 5, 6, 6, 8,
            7, 7, 7, 7, 8, 12, 9, 9, 9, 9, 9, 12, 19, 12, 14, 12, 12, 14, 12, 19, 17,
            20, 16, 15, 16, 20, 17, 30, 23, 21, 21, 23, 30, 34, 29, 27, 29, 34, 42, 37,
            37, 42, 52, 50, 52, 68, 68, 92, 255, 194, 0, 17, 8, 0, 3, 0, 3, 3, 1, 34, 0,
            2, 17, 1, 3, 17, 1, 255, 196, 0, 20, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 6, 255, 218, 0, 8, 1, 1, 0, 0, 0, 0, 123, 255, 196, 0, 20, 1, 1,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 255, 218, 0, 8, 1, 2, 16, 0,
            0, 0, 55, 127, 255, 196, 0, 20, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 7, 255, 218, 0, 8, 1, 3, 16, 0, 0, 0, 21, 255, 196, 0, 28, 16, 0, 2,
            2, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 33, 0, 18, 49, 255,
            218, 0, 8, 1, 1, 0, 1, 63, 0, 192, 90, 157, 241, 85, 153, 159, 125, 165, 26,
            0, 121, 35, 14, 127, 255, 196, 0, 27, 17, 0, 2, 3, 0, 3, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 1, 2, 3, 4, 5, 0, 6, 97, 255, 218, 0, 8, 1, 2, 1, 1, 63, 0,
            214, 236, 27, 201, 171, 166, 137, 183, 125, 85, 109, 204, 0, 22, 100, 0, 0,
            231, 222, 127, 255, 196, 0, 26, 17, 0, 3, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 2, 3, 0, 5, 6, 4, 255, 218, 0, 8, 1, 3, 1, 1, 63, 0, 233, 43,
            89, 116, 91, 249, 74, 140, 147, 77, 143, 169, 85, 84, 144, 170, 162, 164, 0,
            6, 127, 255, 217
        };

        #endregion Samples
    }
}