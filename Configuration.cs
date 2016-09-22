using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class Configuration
    {
        public string InputFile;
        public string DownloadDirectory;
        public string UploadDirectory;
        public string StatusFile;
        public int MaxActiveDownloads;
        public int DefaultDownloadsPerMinute;
        public string pdfHtmlToPdfExePath;    // Added new variable

        Dictionary<string, int> MaxDownloadsPerMinuteDb = new Dictionary<string, int>();

        public int MaxDownloadsPerMinute(string website)
        {
            var host = website.ToLower();

            if (MaxDownloadsPerMinuteDb.ContainsKey(host))
                return MaxDownloadsPerMinuteDb[host];

            return DefaultDownloadsPerMinute;
        }

        public Configuration(string iniFile)
        {
            InputFile = "inputFile.txt";
            StatusFile = "statusFile.txt";
            DownloadDirectory = "downloads";
            UploadDirectory = "uploads";
            pdfHtmlToPdfExePath = "C:\\Program Files\\wkhtmltopdf\\bin\\wkhtmltopdf.exe"; //Added New Value

            MaxActiveDownloads = 5;
            DefaultDownloadsPerMinute = 3;

            MaxDownloadsPerMinuteDb.Add("www.google.com", 1);
            MaxDownloadsPerMinuteDb.Add("wd40.com", 1);
        }
    }
}
