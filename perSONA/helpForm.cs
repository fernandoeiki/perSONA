using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace perSONA
{
    public partial class helpForm : Form
    {
        public helpForm()
        {
            InitializeComponent();
            //this.MinimumSize = new System.Drawing.Size(1100,700);
        }

        private void OpenManual_Click(object sender, EventArgs e)
        {
            var path = Path.Combine("data", "MANUAL-DE-USUARIO.pdf");
            Console.WriteLine(Directory.GetCurrentDirectory());
            string filemanual = path;
            System.Diagnostics.Process.Start(filemanual);
        }
    }
}
//this.MinimumSize = new System.Drawing.Size((1920 * 1100) / x, (1080 * 700) / y);
