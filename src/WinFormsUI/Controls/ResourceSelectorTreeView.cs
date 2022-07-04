#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using ResourcePackerGUI.Domain.Entities;
using WinFormsUI.Extensions;

namespace WinFormsUI.Controls
{
    public partial class ResourceSelectorTreeView : UserControl
    {
        private static readonly Color SelectedBackgroundColor = Color.FromArgb(0, 120, 215);
        private static readonly Color SelectedForegroundColor = Color.White;
        private readonly List<TreeNode> _nodes = new();
        private readonly List<TreeNode> _selectedNodes = new();
        private TreeNode? _selectionPivot;
        private bool _skipNextNodeUpdate;

        public ResourceSelectorTreeView()
        {
            InitializeComponent();
        }

        public event EventHandler<TreeNodeMouseClickEventArgs>? NodeMouseClick;

        public event EventHandler<TreeNodeMouseClickEventArgs>? NodeMouseDoubleClick;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        public void Clear()
        {
            _nodes.Clear();
            treeView.Nodes.Clear();
        }

        public int GetSelectedResourceCount()
        {
            return _selectedNodes.Count;
        }

        public List<Resource> GetSelectedResources()
        {
            var result = new List<Resource>();
            foreach (var node in _selectedNodes)
            {
                List<string> pathNodes;
                if (node.Tag is Resource resource)
                {
                    pathNodes = resource.Name.Split("/").ToList();
                }
                else
                {
                    continue;
                }

                var currentNode = node;
                while (currentNode != null && pathNodes.Count > 1)
                {
                    if (node.Parent != null && !_selectedNodes.Contains(node.Parent))
                    {
                        pathNodes.RemoveAt(pathNodes.Count - 2);
                    }

                    currentNode = currentNode.Parent;
                }

                var name = string.Join("/", pathNodes);
                result.Add(new Resource(resource.Data, resource.Entry, resource.MediaType, name));
            }

            return result;
        }

        public void CreateNodesFromResources(IReadOnlyList<Resource> resources, string packageName,
                    IProgress<int>? progressSecondary = null, int progressReportInterval = 100)
        {
            _selectedNodes.Clear();
            _nodes.Clear();
            if (resources.Count == 0)
            {
                return;
            }

            var rootNode = new TreeNode(packageName)
            {
                ImageIndex = 8,
                SelectedImageIndex = 8
            };

            using (var progressTimer = new System.Timers.Timer(progressReportInterval))
            {
                var percentage = 0;

                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { progressSecondary!.Report(percentage); };
                progressTimer.Enabled = progressSecondary != null;

                for (var i = 0; i < resources.Count; i++)
                {
                    percentage = (int)((double)(i + 1) / resources.Count * 100);
                    var asset = resources[i];
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
                                currentNode.ImageIndex = GetMimeTypeIconIndex(asset.MediaType);
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

            _nodes.Add(rootNode);
            _nodes.AddRange(rootNode.Nodes.CollectAll());

            treeView.Invoke(() =>
            {
                treeView.Nodes.Clear();
                treeView.Nodes.Add(rootNode);
                treeView.ExpandAll();
                rootNode.EnsureVisible();
            });

            // Set the skip variable to false since it is only required
            // after the user requested an expansion or collapse action.
            _skipNextNodeUpdate = false;
        }

        /// <summary>
        /// Gets the corresponding icon for the provided <paramref name="mimeType"/>.
        /// </summary>
        /// <param name="mimeType">A mimetype.</param>
        /// <returns>The index for an icon.</returns>
        private static int GetMimeTypeIconIndex(MediaType? mimeType)
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

        private static void NodeToDefaultColors(TreeNode node)
        {
            node.BackColor = Color.White;
            node.ForeColor = Color.Black;
        }

        private void AddNode(TreeNode node)
        {
            if (!_selectedNodes.Contains(node))
            {
                _selectedNodes.Add(node);
            }
        }

        private void ApplySelectedNodeStyling()
        {
            foreach (var node in _selectedNodes)
            {
                node.BackColor = SelectedBackgroundColor;
                node.ForeColor = SelectedForegroundColor;
            }
        }

        private void ClearSelectedNodeStyling()
        {
            foreach (var node in _selectedNodes)
            {
                NodeToDefaultColors(node);
            }
        }

        private void DeselectNodes(TreeNode node)
        {
            _selectedNodes.Remove(node);

            if (node.Nodes.Count <= 0 || node.Tag != null)
            {
                return;
            }

            foreach (var child in node.Nodes.CollectAll())
            {
                _selectedNodes.Remove(child);
            }
        }

        private void MultiNodeSelectionTreeView_Leave(object sender, EventArgs e)
        {
            ClearSelectedNodeStyling();
            _selectedNodes.Clear();
        }

        private void SelectNodes(TreeNode node)
        {
            AddNode(node);

            if (node.Nodes.Count <= 0 || node.Tag != null)
            {
                return;
            }

            foreach (var child in node.Nodes.CollectAll())
            {
                AddNode(child);
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Disable the default selection style.
            treeView.SelectedNode = null;
        }

        private void TreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            _skipNextNodeUpdate = true;
        }

        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            _skipNextNodeUpdate = true;
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (_skipNextNodeUpdate)
            {
                _skipNextNodeUpdate = false;
                return;
            }

            ClearSelectedNodeStyling();

            var info = treeView.HitTest(e.X, e.Y);

            if (info.Location is TreeViewHitTestLocations.Image or TreeViewHitTestLocations.Label)
            {
                UpdateSelectedNodes(e.Node);
                ApplySelectedNodeStyling();
            }
            else
            {
                _selectedNodes.Clear();
                treeView.SelectedNode = null;
            }

            NodeMouseClick?.Invoke(sender, e);
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var info = treeView.HitTest(e.X, e.Y);
            if (info.Location is not (TreeViewHitTestLocations.Image or TreeViewHitTestLocations.Label))
            {
                ClearSelectedNodeStyling();
                _selectedNodes.Clear();
                return;
            }

            NodeMouseDoubleClick?.Invoke(sender, e);
        }

        private void UpdateSelectedNodes(TreeNode node)
        {
            switch (ModifierKeys)
            {
                case Keys.Shift | Keys.Control when _selectedNodes.Count > 0:
                case Keys.Shift when _selectionPivot != null:
                    var a = _nodes.IndexOf(_selectionPivot!);
                    var b = _nodes.IndexOf(node);

                    if (!ModifierKeys.HasFlag(Keys.Control))
                    {
                        _selectedNodes.Clear();
                    }

                    while (a != b)
                    {
                        AddNode(_nodes[a]);
                        a = a < b ? a + 1 : a - 1;
                    }

                    SelectNodes(_nodes[a]);
                    break;

                case Keys.Control:
                    if (_selectedNodes.Contains(node))
                    {
                        DeselectNodes(node);
                        break;
                    }

                    _selectionPivot = node;
                    SelectNodes(node);
                    break;

                default:
                    _selectionPivot = node;
                    _selectedNodes.Clear();
                    SelectNodes(node);
                    break;
            }
        }
    }
}