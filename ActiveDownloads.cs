using PDFDownloader.ConstantAndEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class ActiveDownloads
    {
        Dictionary<int, Item> Items = new Dictionary<int, Item>();
        public Action<Item> OnChange;
        Logger Logger;


        public int Count { get { return Items.Count; } }

        public ActiveDownloads(Logger logger)
        {
            Logger = logger;
        }

        public void UpdateItemStatus(Item item)
        {

            if (item.Status == EnumItemStatus.Completed)
                Items.Remove(item.Id);
            else
                Items[item.Id] = item;

            Logger.Log(item.Id + " -> " + item.Status.ToString() + " " + item.Message ?? "Started");

            if (OnChange != null)
                OnChange(item);
        }

        public Item[] GetActiveItems()
        {
            return Items.Values.ToArray();
        }
    }
}
