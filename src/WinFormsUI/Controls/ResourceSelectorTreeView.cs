#region GNU General Public License

/* Copyright 2022 Simon Vonhoff & Contributors
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
        private static readonly Color ConflictBackgroundColor = Color.FromArgb(214, 84, 21);
        private static readonly Color ConflictForegroundColor = Color.White;
        private readonly Dictionary<TreeNode, string> _treeNodeNameDictionary = new();
        private TreeNode? _selectionPivot;

        public ResourceSelectorTreeView()
        {
            InitializeComponent();
        }

        public event EventHandler<TreeNodeMouseClickEventArgs>? NodeMouseClick;

        public event EventHandler<TreeNodeMouseClickEventArgs>? NodeMouseDoubleClick;

        public List<TreeNode> Nodes { get; set; } = new();
        public List<TreeNode> SelectedNodes { get; set; } = new();
        public List<Resource> SelectedResources { get; init; } = new();

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
            Nodes.Clear();
            treeView.Nodes.Clear();
        }

        public void CreateNodesFromResources(IReadOnlyList<Resource> resources, string packageName,
                    IProgress<float>? progressSecondary = null, int progressReportInterval = 100)
        {
            Invoke(() => lblConflictAmount.Visible = false);
            SelectedResources.Clear();
            SelectedNodes.Clear();
            Nodes.Clear();

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
                var percentage = 0f;

                // ReSharper disable once AccessToModifiedClosure
                progressTimer.Elapsed += delegate { progressSecondary!.Report(percentage); };
                progressTimer.Enabled = progressSecondary != null;

                for (var i = 0; i < resources.Count; i++)
                {
                    percentage = (i + 1f) / resources.Count * 100f;
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

            Nodes.Add(rootNode);
            Nodes.AddRange(rootNode.Nodes.CollectAll());

            treeView.Invoke(() =>
            {
                treeView.Nodes.Clear();
                treeView.Nodes.Add(rootNode);
                treeView.ExpandAll();
                rootNode.EnsureVisible();
            });
        }

        private void UpdateSelectedResources()
        {
            _treeNodeNameDictionary.Clear();
            SelectedResources.Clear();

            foreach (var node in SelectedNodes)
            {
                if (node.Tag is not Resource resource)
                {
                    continue;
                }

                var name = CreatePathBySelectedNodes(node, resource.Name);
                SelectedResources.Add(new Resource(resource.Data, resource.Entry, resource.MediaType, name));
                _treeNodeNameDictionary.Add(node, name);
            }
        }

        /// <summary>
        /// Creates a path from the starting node and the selected nodes from <see cref="SelectedNodes"/>. <br/>
        /// It filters the deselected nodes from the provided <paramref name="path"/>.
        /// </summary>
        /// <param name="startingNode">The node from where to build a path from.</param>
        /// <param name="path">The raw path as a string.</param>
        /// <returns>A new path created from the selected nodes.</returns>
        private string CreatePathBySelectedNodes(TreeNode startingNode, string path)
        {
            var pathNodes = path.Split("/").ToList();
            var currentNodeParent = startingNode.Parent;
            var offset = 2;
            while (currentNodeParent != null && currentNodeParent != Nodes[0])
            {
                if (!SelectedNodes.Contains(currentNodeParent))
                {
                    pathNodes.RemoveAt(pathNodes.Count - offset);
                }
                else
                {
                    offset++;
                }

                currentNodeParent = currentNodeParent.Parent;
            }

            var name = string.Join("/", pathNodes);
            return name;
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
            if (!SelectedNodes.Contains(node))
            {
                SelectedNodes.Add(node);
            }
        }

        private void ApplySelectedNodeStyling()
        {
            if (SelectedNodes.Count == 0)
            {
                return;
            }

            var conflictingNames = new List<string>();
            var names = SelectedResources.Select(x => x.Name).ToList();
            foreach (var node in SelectedNodes)
            {
                if (_treeNodeNameDictionary.TryGetValue(node, out var name) && names.Count(n => n == name) > 1)
                {
                    if (!conflictingNames.Contains(name))
                    {
                        conflictingNames.Add(name);
                    }

                    node.BackColor = ConflictBackgroundColor;
                    node.ForeColor = ConflictForegroundColor;
                    continue;
                }

                node.BackColor = SelectedBackgroundColor;
                node.ForeColor = SelectedForegroundColor;
            }

            lblConflictAmount.Visible = conflictingNames.Count > 0;
            lblConflictAmount.Text = conflictingNames.Count > 1 ?
                $"{conflictingNames.Count} potential extraction conflicts." :
                "1 potential extraction conflict.";
        }

        private void ClearSelectedNodeStyling()
        {
            foreach (var node in SelectedNodes)
            {
                NodeToDefaultColors(node);
            }
        }

        private void DeselectNodes(TreeNode node)
        {
            SelectedNodes.Remove(node);

            if (node.Nodes.Count <= 0 || node.Tag != null)
            {
                return;
            }

            foreach (var child in node.Nodes.CollectAll())
            {
                SelectedNodes.Remove(child);
            }
        }

        private void MultiNodeSelectionTreeView_Leave(object sender, EventArgs e)
        {
            ResetSelectedValues();
        }

        private void ResetSelectedValues()
        {
            ClearSelectedNodeStyling();
            SelectedNodes.Clear();
            SelectedResources.Clear();
            lblConflictAmount.Visible = false;
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

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var info = treeView.HitTest(e.X, e.Y);
            switch (info.Location)
            {
                case TreeViewHitTestLocations.PlusMinus:
                {
                    return;
                }

                case TreeViewHitTestLocations.Image or
                     TreeViewHitTestLocations.Label or
                     TreeViewHitTestLocations.Indent:
                {
                    ClearSelectedNodeStyling();
                    UpdateSelectedNodes(e.Node);
                    UpdateSelectedResources();
                    ApplySelectedNodeStyling();
                    break;
                }

                default:
                {
                    ResetSelectedValues();
                    break;
                }
            }

            NodeMouseClick?.Invoke(sender, e);
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var info = treeView.HitTest(e.X, e.Y);

            if (info.Location is TreeViewHitTestLocations.PlusMinus)
            {
                return;
            }

            if (info.Location is not (
                TreeViewHitTestLocations.Image or
                TreeViewHitTestLocations.Label or
                TreeViewHitTestLocations.Indent))
            {
                ResetSelectedValues();
                return;
            }

            NodeMouseDoubleClick?.Invoke(sender, e);
        }

        private void UpdateSelectedNodes(TreeNode node)
        {
            switch (ModifierKeys)
            {
                case Keys.Shift | Keys.Control when SelectedNodes.Count > 0:
                case Keys.Shift when _selectionPivot != null:
                {
                    var a = Nodes.IndexOf(_selectionPivot!);
                    var b = Nodes.IndexOf(node);

                    if (!ModifierKeys.HasFlag(Keys.Control))
                    {
                        SelectedNodes.Clear();
                    }

                    while (a != b)
                    {
                        AddNode(Nodes[a]);
                        a = a < b ? a + 1 : a - 1;
                    }

                    SelectNodes(Nodes[a]);
                    break;
                }

                case Keys.Control:
                {
                    if (SelectedNodes.Contains(node))
                    {
                        DeselectNodes(node);
                        break;
                    }

                    _selectionPivot = node;
                    SelectNodes(node);
                    break;
                }

                default:
                {
                    _selectionPivot = node;
                    SelectedNodes.Clear();
                    SelectNodes(node);
                    break;
                }
            }
        }
    }
}