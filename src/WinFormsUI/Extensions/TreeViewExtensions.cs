namespace WinFormsUI.Extensions
{
    internal static class TreeViewExtensions
    {
        internal static IEnumerable<TreeNode> CollectAll(this TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in CollectAll(node.Nodes))
                {
                    yield return child;
                }
            }
        }
    }
}