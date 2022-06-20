using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IMimeTypeService
    {
        MediaType? GetTypeByData(byte[] data);

        MediaType? GetTypeByName(string name);
    }
}