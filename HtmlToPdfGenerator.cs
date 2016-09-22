using PDFDownloader.ConstantAndEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class HtmlToPdfGenerator
    {
        ActiveDownloads ActiveDownloads;
        Output Output;
        Configuration Configuration;
        DirectoryMonitor DirectoryMonitor;

        public HtmlToPdfGenerator(Output output, Configuration configuration, ActiveDownloads activeDownloads, DirectoryMonitor directoryMonitor)
        {
            ActiveDownloads = activeDownloads;
            Output = output;
            Configuration = configuration;
            DirectoryMonitor = directoryMonitor;
        }
        public int HtmlToPdfDownloads(string Uri, int Id)
        {
            bool ResponseUrlStatus = false;
            int returnCode = 1;
            try
            {
                ResponseUrlStatus = CheckUrlStatus(Uri.ToString());
                if (ResponseUrlStatus == true)
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
                    returnCode = p.ExitCode;
                    p.Close();
                    // if 0 or 2, it worked so return status of file
                }
                return returnCode;
            }
            catch (Exception exc)
            {
                return 1;
                //throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
            }
        }
        protected bool CheckUrlStatus(string Website)
        {
            try
            {
                var request = WebRequest.Create(Website) as HttpWebRequest;
                request.Method = "HEAD";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
