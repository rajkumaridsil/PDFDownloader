using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class DirectoryMonitor
    {
        string InputDirectory;
        Input Input;
        public DirectoryMonitor(string inputDirectory, Input input)
        {
            InputDirectory = inputDirectory;
            Input = input;
        }

        public void Watcher()
        {
            DirectoryInfo directory = new DirectoryInfo(InputDirectory);
            FileInfo[] Files = directory.GetFiles("*.txt");// get only text files
            foreach (FileInfo file in Files)// if we have multiple files
            {
                var inputFile = Path.Combine(InputDirectory, file.Name);
                Input.AddNewItems(inputFile);
                file.Delete();// delete the file from the directory
            }
        }
    }
}
