using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDA.Controllers
{
    public class StaffController : Controller
    {   
        [HttpGet]
        public ActionResult StaffPortal()
        {
            return View();
            
        }
        [HttpPost]
        public ActionResult StaffPortal(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Files/Excel Files"),
                    Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();

        }
        public ActionResult Progress()
        {
            return View();
        }

    }
}