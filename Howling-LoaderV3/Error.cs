using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Howling_LoaderV3
{
    internal partial class Error : Form
    {
        public Error()
        {
            InitializeComponent();
        }

        public Error(string title)
        {
            InitializeComponent();

            this.ErrorMsg.Text = title;
        }

        public static class CstmError
        {
            public static void Show(string title)
            {
                using (var form = new Error(title))
                {
                    form.ShowDialog();
                }
            }
        }

        private void Error_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Exclamation.Play();
        }

        private void Launch_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ErrorMsg_Click(object sender, EventArgs e)
        {

        }

        private void Launch_button_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
