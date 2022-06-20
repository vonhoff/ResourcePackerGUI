using ResourcePackerGUI.Domain.Common;

namespace ResourcePackerGUI.Domain.ValueObjects
{
    public class MediaType : ValueObject
    {
        public string Name => $"{PrimaryType}/{SubType}";
        public string Description { get; init; } = string.Empty;
        public string[] Extensions { get; init; } = Array.Empty<string>();
        public string PrimaryType { get; init; } = string.Empty;
        public string SubType { get; init; } = string.Empty;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}