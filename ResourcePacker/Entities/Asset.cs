

using Winista.Mime;

namespace ResourcePacker.Entities
{
    public class Asset
    {
        private string _name = string.Empty;

        public Asset(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; init; }

        public Entry Entry { get; set; }

        public MimeType? MimeType { get; set; }

        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                {
                    return _name;
                }

                var name = Entry.Id.ToString();
                var extension = MimeType?.Extensions.FirstOrDefault();

                if (extension == null && !string.IsNullOrWhiteSpace(MimeType?.SubType))
                {
                    extension = MimeType.SubType;
                }

                if (extension != null)
                {
                    name += $".{extension}";
                }

                return name;
            }
            set => _name = value;
        }
    }
}