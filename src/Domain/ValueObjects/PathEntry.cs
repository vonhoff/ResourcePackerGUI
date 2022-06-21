using ResourcePackerGUI.Domain.Common;

namespace ResourcePackerGUI.Domain.ValueObjects
{
    public class PathEntry : ValueObject
    {
        public PathEntry(string absolutePath, string relativePath)
        {
            AbsolutePath = absolutePath;
            RelativePath = relativePath;
        }

        public string AbsolutePath { get; set; }

        public string RelativePath { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AbsolutePath;
            yield return RelativePath;
        }
    }
}