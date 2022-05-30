using ResourcePacker.Entities;
using ResourcePacker.Helpers;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.RichTextBoxForms.Themes;

namespace ResourcePacker.Forms
{
    public partial class CreateForm : Form
    {
        public CreateForm()
        {
            InitializeComponent();
        }

        private void CreateForm_SizeChanged(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance =
                Math.Min(splitContainer2.SplitterDistance, Width - splitContainer2.Panel2MinSize);
        }
    }
}