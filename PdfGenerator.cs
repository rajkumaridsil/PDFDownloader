using PDFDownloader.ConstantAndEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class PdfGenerator
    {
        ActiveDownloads ActiveDownloads;
        Output Output;
        Configuration Configuration;
        DirectoryMonitor DirectoryMonitor;
        string LastStatusText;

        public PdfGenerator(Output output, Configuration configuration, ActiveDownloads activeDownloads, DirectoryMonitor directoryMonitor)
        {
            ActiveDownloads = activeDownloads;
            Output = output;
            Configuration = configuration;
            DirectoryMonitor = directoryMonitor;
        }
        public int HtmlToPdfDownloads(string Uri,int Id)
        {
             try
            {
           var p = new System.Diagnostics.Process()
           {
                    StartInfo =
                    {
                        FileName = Configuration.pdfHtmlToPdfExePath,
                        Arguments = Uri + " " + Output.GenrateOutputFilename(Id),
                        UseShellExecute = false, // needs to be false in order to redirect output
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                        WorkingDirectory = Output.GenrateOutputPath(Output)
                    }
                };
                p.Start();
                // read the output here...
                var output = p.StandardOutput.ReadToEnd();
                var errorOutput = p.StandardError.ReadToEnd();
                int returnCode = p.ExitCode;
                p.Close();
               // if 0 or 2, it worked so return status of file
                return returnCode;
            }
             catch (Exception exc)
             {
                 return 1;
                 //throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
             }
        }
    }
}
