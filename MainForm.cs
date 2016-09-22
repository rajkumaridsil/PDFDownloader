using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFDownloader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        public void Log(string msg)
        {
            tbLog.AppendText(msg + Environment.NewLine);
        }

        public void SetActiveList(string[] lines)
        {

            tbActiveList.AppendText(string.Join(Environment.NewLine, lines));
        }
    }
}
