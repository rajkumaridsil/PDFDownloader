using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class Output
    {
        string OutputDirectory;

        public Output(string outputDirectory)
        {
            OutputDirectory = outputDirectory;
        }

        public void Save(int id, string url, bool success, string message, byte[] data)
        {
            // --- example code ---

            var textFile = Path.Combine(OutputDirectory, string.Format("{0}.txt", id));
           // var pdfFile = Path.Combine(OutputDirectory, string.Format("{0}.pdf", id));
            var text = string.Format("{0},{1}", id, message);

            File.WriteAllText(textFile, text);

            //if (success)
            //    File.WriteAllBytes(pdfFile, data);
        }
        //add new funcation for genrate new file name
        public string GenrateOutputFilename(int Id)
        {
            string outputFilename = Id + ".PDF"; // assemble destination PDF file name
            return outputFilename;
        }
        public string GenrateOutputPath(Output OutputDirectory)
        {
            string WorkingDirectory;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            WorkingDirectory = baseDirectory + "" + OutputDirectory.OutputDirectory;
            return WorkingDirectory;
        }
    }
}
