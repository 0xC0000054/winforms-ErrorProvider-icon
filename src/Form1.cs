using TerraFX.Interop.Windows;

namespace ErrorProviderIcon
{
    public partial class Form1 : Form
    {
        private Icon defaultErrorProviderIcon;

        public Form1()
        {
            InitializeComponent();
            defaultErrorProviderIcon = (Icon)errorProvider1.Icon.Clone();
        }

        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                errorProvider1.SetError(textBox1, "The text box must be empty.");
                e.Cancel = true;
            }
        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                errorProvider1.SetError(textBox1, null);
            }
        }

        private unsafe void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                const SHSTOCKICONID siid = SHSTOCKICONID.SIID_ERROR;
                const uint uFlags = SHGSI.SHGSI_ICON | SHGSI.SHGSI_SMALLICON;
                SHSTOCKICONINFO sii = new();
                sii.cbSize = SHSTOCKICONINFO.SizeOf;

                int hr = Windows.SHGetStockIconInfo(siid, uFlags, &sii);

                if (Windows.SUCCEEDED(hr))
                {
                    try
                    {                    
                        Icon icon = Icon.FromHandle((IntPtr)sii.hIcon.Value);

                        errorProvider1.Icon = (Icon)icon.Clone();
                    }
                    finally
                    {
                        Windows.DestroyIcon(sii.hIcon);
                    }
                }
            }
            else
            {
                if (defaultErrorProviderIcon != null)
                {
                    errorProvider1.Icon = defaultErrorProviderIcon;
                }
            }
        }
    }
}