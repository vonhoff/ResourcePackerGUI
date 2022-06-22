using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IMimeTypeService
    {
        MediaType? GetTypeByData(byte[] data);

        MediaType? GetTypeByName(string name);
    }
}