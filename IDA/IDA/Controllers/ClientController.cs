using IDA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDA.Controllers
{
    public class ClientController : Controller
    {
        IdaDBEntities db = new IdaDBEntities();
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Details()
        {
            return View();
        }
        public ActionResult newProject()
        {
            return View(); 
        }
        public ActionResult ExistingProject()
        {
            return View();
        }
        public ActionResult ClientProfile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectRequest([Bind(Include = "id,ProjectName,DateRequested,ClientId")] Project_Request pr)
        {
            if (ModelState.IsValid)
            {
                db.Project_Request.Add(pr);
                
                db.SaveChanges();
                return RedirectToAction("Index", "Client");
            }
            return View();
        }
    }
}