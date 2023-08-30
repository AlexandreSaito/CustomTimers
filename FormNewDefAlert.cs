using System;
using System.Windows.Forms;

namespace T
{
    public partial class FormNewDefAlert : Form
    {
        public string TextValue = "";
        public FormNewDefAlert()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            this.DialogResult = DialogResult.OK;
            TextValue = txtName.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
