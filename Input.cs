using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class Input
    {
        List<Row> Rows;

        public class Row
        {
            public int Id;
            public string Uri;
        }

        public Input(string inputFile)
        {
            Rows = File.ReadAllLines(inputFile)
                .Select(x => x.Split(','))
                .Select(x => new Row { Id = int.Parse(x[0]), Uri = x[1] })
                .ToList();
        }

        public Row GetNextRow()
        {
            if (Rows.Count > 0)
            {
                var row = Rows[0];
                Rows.RemoveAt(0);
                return row;
            }

            return null; // no more rows
        }

        public void AddNewItems(string inputFile)
        {
            Rows.AddRange(File.ReadAllLines(inputFile)
                .Select(x => x.Split(','))
                .Select(x => new Row { Id = int.Parse(x[0]), Uri = x[1] })
                .ToList());
        }
    }
}
