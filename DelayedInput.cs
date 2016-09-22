using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class DelayedInput
    {
        Input Input;
        Configuration Configuration;
        Dictionary<string, UriQueue> QueueMap = new Dictionary<string, UriQueue>();

        private class UriQueue
        {
            Queue<Input.Row> Queue = new Queue<Input.Row>();
            public int LastTick;
            public int Delay;

            public UriQueue(int delay)
            {
                Delay = delay;
                LastTick = 0;
            }

            public Input.Row GetNextRow(int currTick)
            {
                if (Queue.Count > 0 && (currTick - LastTick) >= Delay)
                {
                    LastTick = currTick; // mark the start time
                    return Queue.Dequeue();
                }
                return null;
            }

            public void Add(Input.Row row)
            {
                Queue.Enqueue(row);
            }
        }

        int GetDelay(string host)
        {
            var maxPerMinute = Configuration.MaxDownloadsPerMinute(host);
            return 60000 / maxPerMinute; // delay in milliseconds between downloads from this host
        }

        UriQueue GetQueue(string host)
        {
            if (QueueMap.ContainsKey(host))
                return QueueMap[host];

            // it's a new host, create the queue

            var delay = GetDelay(host);
            var queue = new UriQueue(delay);
            QueueMap.Add(host, queue);

            return queue;
        }

        void AddRow(Input.Row row)
        {
            var uri = row.Uri;
            var host = new Uri(uri).Host.ToLower();
            var queue = GetQueue(host);
            queue.Add(row);
        }

        void RefillFromInput()
        {
            // add the new downloads coming from Input
            for (var nextRow = Input.GetNextRow(); nextRow != null; nextRow = Input.GetNextRow())
            {
                AddRow(nextRow);
            }
        }

        Input.Row GetFirstReady()
        {
            // get the first row ready to download

            var currTick = Environment.TickCount;

            foreach (var entry in QueueMap)
            {
                var queue = entry.Value;
                var row = queue.GetNextRow(currTick);
                if (row != null)
                    return row;
            }
            // none available
            return null;
        }

        public Input.Row GetNextRow()
        {
            RefillFromInput();
            return GetFirstReady();
        }

        public DelayedInput(Input source, Configuration configuration)
        {
            Input = source;
            Configuration = configuration;
        }
    }
}
