using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace IDA.Models
{
    public class DownloadFile
    {
        public List<Project> GetFiles()
        {
            List<Project> lstFiles = new List<Project>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/MD/img"));

            int i = 0;
            String path = "";
            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new Project()
                {
                    FilePath = dirInfo.FullName + @"\" + item.Name
               
                });
                i = i + 1;
            }
            return lstFiles;
        }
        public List<Transaction> GetSlip()
        {
            List<Transaction> lstFiles = new List<Transaction>();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Files/Slips"));

            int i = 0;
            String path = "";
            foreach (var item in dirInfo.GetFiles())
            {
                lstFiles.Add(new Transaction()
                {
                    SlipPath = dirInfo.FullName + @"\" + item.Name

                });
                i = i + 1;
            }
            return lstFiles;
        }
    }
}