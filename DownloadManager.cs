using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class DownloadManager
    {
        ActiveDownloads ActiveDownloads;
        Output Output;
        DelayedInput DelayedInput;
        DirectoryMonitor DirectoryMonitor;
        Configuration Configuration;
        HtmlToPdfGenerator PdfGenerator;
        string LastStatusText;

        public DownloadManager(DelayedInput input, Output output, Configuration configuration, ActiveDownloads activeDownloads, Logger logger, DirectoryMonitor directoryMonitor, HtmlToPdfGenerator pdfgenerator)
        {
            ActiveDownloads = activeDownloads;
            Output = output;
            DelayedInput = input;
            DirectoryMonitor = directoryMonitor;
            Configuration = configuration;
            PdfGenerator = pdfgenerator;

            StartTimer(100, StartDownloads); // start new downloads if possible (check every 100 msec)
            StartTimer(30000, ReportStatus); // write the status every 30 secs
            StartTimer(5000, AddNewItems); // check the directory for new downloads at run time (check every 5 secs)
        }

        private void StartTimer(int interval, Action action)
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = interval;
            timer.Tick += (obj, args) => action();
            timer.Enabled = true;
        }


        public void StartDownloads()
        {
            int active = ActiveDownloads.Count;
            int maxActive = Configuration.MaxActiveDownloads;

            while (active < maxActive)
            {
                var row = DelayedInput.GetNextRow();
                if (row == null) // no items to download
                    break;

                var item = new Item(row, ActiveDownloads, Output, PdfGenerator);
                item.Start(); // start the download
                active++;
            }
        }
        public void ReportStatus()
        {
            var text = string.Join(Environment.NewLine,
                ActiveDownloads.GetActiveItems().Select(item => string.Format("{0},{1}", item.Id, item.Uri)));

            if (text != LastStatusText)
            {
                LastStatusText = text;
                File.WriteAllText(Configuration.StatusFile, text);
            }
        }

        public void AddNewItems()
        {
            DirectoryMonitor.Watcher();
        }
    }
}
