using ResourcePacker.Controls;
using Winista.Mime;

namespace ResourcePacker.Forms
{
    public partial class CreateForm : Form
    {
        public CreateForm()
        {
            InitializeComponent();
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void BtnAssetExplore_Click(object sender, EventArgs e)
        {
            using var browserDialog = new FolderBrowserDialog();
            var result = browserDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrWhiteSpace(browserDialog.SelectedPath))
            {
                return;
            }

            txtAssetFolder.Text = browserDialog.SelectedPath;

            var assetPathNodes = browserDialog.SelectedPath.Replace(@"\", "/").Split('/');
            var rootNode = new TreeNode(assetPathNodes.Last() + " (root)")
            {
                Checked = true,
                StateImageIndex = 1
            };

            var files = Directory.GetFiles(browserDialog.SelectedPath,
                string.Empty, SearchOption.AllDirectories);

            lblAvailableItems.Text = $"Available items: {files.Length}";

            foreach (var path in files)
            {
                var currentNode = rootNode;
                var pathNodes = path.Replace(@"\", "/").Split('/');
                for (var j = assetPathNodes.Length; j < pathNodes.Length; j++)
                {
                    var item = pathNodes[j];
                    var folder = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(x => x.Text.Equals(item));

                    if (folder != null)
                    {
                        currentNode = folder;
                    }
                    else
                    {
                        currentNode = currentNode.Nodes.Add(item);

                        if (j == pathNodes.Length - 1)
                        {
                            currentNode.Tag = path;
                        }
                    }
                }
            }

            explorerTreeView.Invoke(() =>
            {
                explorerTreeView.Nodes.Clear();
                if (rootNode.GetNodeCount(true) < 1)
                {
                    return;
                }

                explorerTreeView.Nodes.Add(rootNode);
                explorerTreeView.ExpandAll();
                explorerTreeView.Nodes[0].EnsureVisible();
            });
        }

        private void BtnPackageExplore_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ResourcePack (*.dat)|*.dat";
            var result = saveFileDialog.ShowDialog();

            if (result != DialogResult.OK || string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                return;
            }

            txtPackageLocation.Text = saveFileDialog.FileName;
        }
    }
}