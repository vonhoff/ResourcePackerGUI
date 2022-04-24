namespace ResourcePacker.Entities
{
    internal class EntryDefinition
    {
        private string _name = string.Empty;

        public string Name
        {
            get => string.IsNullOrEmpty(_name) ? Entry.Id.ToString() : _name;
            set => _name = value;
        }

        public Entry Entry { get; set; }
    }
}