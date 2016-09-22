using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class Logger
    {
        MainForm MainForm;

        public Logger(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        string FormatLine(string message)
        {
            var now = DateTime.Now;
            return string.Format("{0}@{1}:  {2}", now.ToShortDateString(), now.ToLongTimeString(), message);
        }

        public void Log(string message)
        {
            string line = FormatLine(message);
            MainForm.Log(line);
        }
    }
}
