using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFDownloader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new MainForm();

            var configuration = new Configuration("config.ini");
            var logger = new Logger(form);
            var input = new Input(configuration.InputFile);
            var delayedInput = new DelayedInput(input, configuration);
            var output = new Output(configuration.DownloadDirectory);
            var activeDownloads = new ActiveDownloads(logger);
            var directoryMonitor = new DirectoryMonitor(configuration.UploadDirectory, input);
            var PdfGenerator = new HtmlToPdfGenerator(output, configuration, activeDownloads, directoryMonitor);

            activeDownloads.OnChange = (item) =>
            {
                var lines = activeDownloads.GetActiveItems()
                    .Select(x => string.Format("{0} {1}", x.Id, x.Uri))
                    .ToArray();

                form.SetActiveList(lines);
            };

            //#if DEBUG
            //// remove this line in production
            //Directory.CreateDirectory(configuration.DownloadDirectory);
            //Directory.CreateDirectory(configuration.UploadDirectory);
            //#endif
            var DownloadManager = new DownloadManager(delayedInput, output, configuration, activeDownloads, logger, directoryMonitor, PdfGenerator);




            Application.Run(form);
        }
    }
}
