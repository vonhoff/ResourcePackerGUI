using System.Data;
using System.Diagnostics;
using ResourcePacker.Entities;
using Serilog;
using Winista.Mime;

namespace ResourcePacker.Controls
{
    public partial class MultiNodeSelectionTreeView : UserControl
    {
        private List<TreeNode> _nodes = new List<TreeNode>();
        private static readonly Color SelectedBackgroundColor = Color.FromArgb(0, 120, 215);
        private static readonly Color SelectedForegroundColor = Color.White;

        public MultiNodeSelectionTreeView()
        {
            InitializeComponent();
        }

        public event EventHandler<TreeNodeMouseClickEventArgs>? NodeMouseDoubleClick;

        public List<TreeNode> SelectedNodes { get; private set; } = new();

        public void CreateNodesFromAssets(IReadOnlyList<Asset> assets, string packageName,
            IProgress<int>? progressSecondary = null, int progressReportInterval = 100)
        {
            _nodes.Clear();
            if (assets.Count == 0)
            {
                return;
            }

            var rootNode = new TreeNode(packageName)
            {
                ImageIndex = 8,
                SelectedImageIndex = 8
            };

            using (var timer = new System.Timers.Timer(progressReportInterval))
            {
                var percentage = 0;
                timer.Elapsed += delegate { progressSecondary!.Report(percentage); };
                timer.Enabled = progressSecondary != null;

                for (var i = 0; i < assets.Count; i++)
                {
                    percentage = (int)((double)(i + 1) / assets.Count * 100);
                    var asset = assets[i];
                    var path = asset.Name;
                    var currentNode = rootNode;
                    var pathNodes = path.Split('/');
                    for (var j = 0; j < pathNodes.Length; j++)
                    {
                        var item = pathNodes[j];
                        var folder = currentNode.Nodes.Cast<TreeNode>()
                            .FirstOrDefault(n => n.Text.Equals(item));

                        if (folder != null)
                        {
                            currentNode = folder;
                            currentNode.ImageIndex = 7;
                        }
                        else
                        {
                            currentNode = currentNode.Nodes.Add(item);
                            
                            if (j == pathNodes.Length - 1)
                            {
                                currentNode.ImageIndex = GetMimeTypeIconIndex(asset.MimeType);
                                currentNode.Tag = asset;
                            }
                            else
                            {
                                currentNode.ImageIndex = 7;
                            }
                        }

                        currentNode.SelectedImageIndex = currentNode.ImageIndex;
                    }
                }
            }

            _nodes.AddRange(Collect(rootNode.Nodes));

            treeView.Invoke(() =>
            {
                treeView.Nodes.Clear();
                treeView.Nodes.Add(rootNode);
                treeView.ExpandAll();
                rootNode.EnsureVisible();
            });
        }

        private static IEnumerable<TreeNode> Collect(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;

                foreach (var child in Collect(node.Nodes))
                {
                    yield return child;
                }
            }
        }

        /// <summary>
        /// Gets the corresponding icon for the provided <paramref name="mimeType"/>.
        /// </summary>
        /// <param name="mimeType">A mimetype.</param>
        /// <returns>The index for an icon.</returns>
        private static int GetMimeTypeIconIndex(MimeType? mimeType)
        {
            if (mimeType == null)
            {
                return 6;
            }

            return mimeType.PrimaryType switch
            {
                "application" => 0,
                "audio" => 1,
                "font" => 2,
                "image" => 3,
                "text" => 4,
                "video" => 5,
                _ => 6
            };
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ClearSelectedNodeStyling();

            if (e.Node.Bounds.Contains(e.X, e.Y))
            {
                UpdateSelectedNodes(e.Node);
                ApplySelectedNodeStyling();
            }
            else
            {
                SelectedNodes.Clear();
                treeView.SelectedNode = null;
            }
        }

        private void UpdateSelectedNodes(TreeNode node)
        {
            switch (ModifierKeys)
            {
                case Keys.Control:
                    SelectedNodes.Add(node);
                    break;
                case Keys.Shift when SelectedNodes.Count > 0:
                    var a = _nodes.IndexOf(SelectedNodes[0]);
                    var b = _nodes.IndexOf(node);

                    SelectedNodes.Clear();
                    while (a != b)
                    {
                        SelectedNodes.Add(_nodes[a]);
                        a = a < b ? a + 1 : a - 1;
                    }
                    break;
                default:
                    SelectedNodes.Clear();
                    SelectedNodes.Add(node);
                    break;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            NodeMouseDoubleClick?.Invoke(sender, e);
        }

        private void MultiNodeSelectionTreeView_Leave(object sender, EventArgs e)
        {
            ClearSelectedNodeStyling();
            SelectedNodes.Clear();
        }

        private void ClearSelectedNodeStyling()
        {
            foreach (var node in SelectedNodes)
            {
                node.BackColor = Color.White;
                node.ForeColor = Color.Black;
            }
        }

        private void ApplySelectedNodeStyling()
        {
            foreach (var node in SelectedNodes)
            {
                node.BackColor = SelectedBackgroundColor;
                node.ForeColor = SelectedForegroundColor;
            }
        }
    }
}