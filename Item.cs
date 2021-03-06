﻿using PDFDownloader.ConstantAndEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PDFDownloader
{
    public class Item
    {
        public string Uri;
        public int Id;
        public EnumItemStatus Status;
        public byte[] Data;
        public string Message;
        public bool Success;
        public Exception Error;
        public DateTime DownloadedTime;

        ActiveDownloads ActiveDownloads;
        HtmlToPdfGenerator HtmlToPdfGenerator;
        Output Output;

        public Item(Input.Row row, ActiveDownloads activeDownloads, Output output, HtmlToPdfGenerator htmltoPdfgenerator)
        {
            Id = row.Id;
            Uri = row.Uri;
            ActiveDownloads = activeDownloads;
            Output = output;
            HtmlToPdfGenerator = htmltoPdfgenerator;
        }

        private void SetStatus(EnumItemStatus status)
        {
           
            Status = status; 
            ActiveDownloads.UpdateItemStatus(this);
            if (Status == EnumItemStatus.Completed)
            {
                this.DownloadedTime = DateTime.Now;
            }
        }

        private void HtmlToPdfGeneratorThread()
        {
            int returnCode;
     
             returnCode = HtmlToPdfGenerator.HtmlToPdfDownloads(Uri.ToString(), Id);
               if ((returnCode == 0) || (returnCode == 2))
               {
                   Message = "200 OK";
                   Success = true;
               }
               else
               {
                   Message = "0 Not a .html";
                   Success = false;
               }
           
       }
       public async void Start()
       {
            SetStatus(EnumItemStatus.Working);


            await Task.Run(() => HtmlToPdfGeneratorThread());

            SetStatus(EnumItemStatus.Completed);

            Output.Save(Id, Uri, Success, Message, Data);
        }

        static Item()
        {
            // fix a slowdown of WebClient
            WebRequest.DefaultWebProxy = null;
        }
        public void WriteCurrentStatus()
        {
            SetStatus(EnumItemStatus.Working);
        }

    }
}
