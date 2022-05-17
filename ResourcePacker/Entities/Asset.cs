using Winista.Mime;

namespace ResourcePacker.Entities
{
    public class Asset
    {
        private string _name = string.Empty;

        public string Name
        {
            get => string.IsNullOrEmpty(_name) ? Entry.Id.ToString() : _name;
            set => _name = value;
        }

        public Entry Entry { get; set; }

        public byte[] Data { get; set; }

        public MimeType? MimeType { get; set; }
    }
}