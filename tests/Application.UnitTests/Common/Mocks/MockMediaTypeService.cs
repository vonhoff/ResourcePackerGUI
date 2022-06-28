using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Domain.Entities;

namespace Application.UnitTests.Common.Mocks
{
    internal class MockMediaTypeService : IMediaTypeService
    {
        public MediaType? GetTypeByData(byte[] data)
        {
            return null;
        }

        public MediaType? GetTypeByName(string name)
        {
            return null;
        }
    }
}