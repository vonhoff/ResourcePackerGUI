namespace ResourcePackerGUI.Domain.Entities
{
    public class PathEntry
    {
        /// <summary>
        /// Constructor for the <see cref="PathEntry"/> class.
        /// </summary>
        /// <param name="absolutePath">The full path towards the file location.</param>
        /// <param name="relativePath">The relative path from the specified root folder.</param>
        public PathEntry(string absolutePath, string relativePath)
        {
            AbsolutePath = absolutePath;
            RelativePath = relativePath;
        }

        /// <summary>
        /// The full path towards the file location.
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// The relative path from the specified root folder.
        /// </summary>
        public string RelativePath { get; set; }
    }
}