using IDA.Models;
using Microsoft.Graph;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EmailAddress = SendGrid.Helpers.Mail.EmailAddress;

namespace IDA.Controllers
{
    public class ClientController : Controller
    {
        private IdaDBEntities db = new IdaDBEntities();
        // GET: Client
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult AllProjects()
        {
            return View(db.Projects.ToList());
        }
        public ActionResult newProject()
        {
            //client.ClientId= Convert.ToInt32(Session["ClientId"]);
            return View(); 
        }
        [HttpPost]
        public ActionResult newProject(FormCollection f, HttpPostedFileBase file)
        {

           // user.UserId = Convert.ToInt32(Session["UserId"]);
            if (ModelState.IsValid)
            {
               
                    Project p = new Project();
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path1 = Path.Combine(Server.MapPath("~/Files/ProjectRequests"), fileName);
                        file.SaveAs(path1);
                        p.FilePath = path1;
                        p.ProjectName = f["ProjectName"];
                        p.DateRequested = DateTime.Now;
                        p.ClientId = Convert.ToInt32(Session["ClientId"]);
                        p.Description = f["Description"];
                        p.Status = "Pending";
                        p.ProjectType = "Client Request";
                            p.AdvertDate = Convert.ToDateTime(f["AdvertDate"]);
                            p.BriefingDate = Convert.ToDateTime(f["BriefingDate"]);
                            p.SubmitionDate = Convert.ToDateTime(f["SubmitionDate"]);
                    try
                    {

                     
                        db.Projects.Add(p);
                        db.SaveChanges();
                        ViewBag.Message = "Submitted successfully";
                    }
                    catch
                    {

                    }
                }
            }
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
        public ActionResult Meeting()
        {
            return View();
        }
        public JsonResult GetMeetings(int? id)
        {
            id = Convert.ToInt32(Session["ClientId"]);
            using (IdaDBEntities dc = new IdaDBEntities())
            {
                var meetings = dc.Meetings.ToList();
                List<Meeting> meet = new List<Meeting>();
                foreach (var item in meetings)
                {
                    if (item.ClientId == id)
                    {
                        meet.Add(item);
                    }
                }
                return new JsonResult { Data = meet, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult AddMeeting()
        {
            return View();
        }

        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddMeeting(Meeting user)
        {
            // int EmpId = Convert.ToInt32(Session["EmpId"]);
            if (ModelState.IsValid)
            {
                IdaDBEntities db = new IdaDBEntities();
                user.TDate = DateTime.Now;
                user.EmpId = null;
                user.ClientId = Convert.ToInt32(Session["ClientId"]);
                db.Meetings.Add(user);
                db.SaveChanges();
                switch (user.MeetingId)
                {
                    case -1:
                        break;
                    case -2:
                        break;
                    default:
                        SendEmail(user);
                        break;
                }

            }
            else
            {
                ViewBag.MessageError = "Error!!";
            }
            return RedirectToAction("Meeting", "Client");
        }

        public void SendEmail(Meeting user)
        {
            using (var ctx = new IdaDBEntities())
            {

                var StartDate = ctx.Database.SqlQuery<DateTime>("select StartDate from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var EndDate = ctx.Database.SqlQuery<DateTime>("SELECT EndDate from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var Subject = ctx.Database.SqlQuery<string>("SELECT Subject from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var Description = ctx.Database.SqlQuery<string>("SELECT Description from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();

                //var apiKey = Environment.GetEnvironmentVariable("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");
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("Izingodla@gmail.com", "Meeting Invitation");
                var Emailsubject = "Meeting Invitation";
                foreach (var address in user.NewClientEmail.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var to = new EmailAddress(address);

                    var plainTextContent = "Meeting Invitation";
                    string htmlContent = "Dear Sir/Ma’am,";
                    htmlContent += "<br /><br />You are being invited to the meeting by Izingodla Engineering.";
                    htmlContent += "<br /><br /> Subject : " + Subject + "";
                    htmlContent += "<br /> Location : " + Description + "";
                    htmlContent += "<br /> Start Time : " + StartDate + "";
                    htmlContent += "<br /> End Time : " + EndDate + "";
                    htmlContent += "<br /><br /> We would be awaiting your esteemed presence in the meeting. Please feel free to contact us anytime, if you require any change in the meeting schedule.";
                    htmlContent += "<br /><br />Thanks <br /> Izingodla Team";
                    var msg = MailHelper.CreateSingleEmail(from, to, Emailsubject, plainTextContent, htmlContent);
                    var response = client.SendEmailAsync(msg);
                }
            }
        }
        public ActionResult ActiveProjects()
        {
            var activeProjects = db.Projects.ToList();
            List<Project> activelist = new List<Project>();

            foreach (var item in activeProjects)
            {
                if (item.ClientId.Equals(Session["ClientId"]) && item.ClientId != 0)
                {
                    activelist.Add(item);
                }
            }
            return View(activelist);
        }
        public ActionResult RejectedProjects()
        {

            var rejectedProjects = db.Projects.ToList();
            List<Project> rejectedlist = new List<Project>();

            foreach (var item in rejectedProjects)
            {
                if (item.ClientId.Equals(Session["ClientId"]) && item.ClientId !=0)
                {
                    rejectedlist.Add(item);
                }
            }
            return View(rejectedlist);
        }
        public ActionResult rejectedProjectDetails(int? id)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("Select * from Project where ProjectId = " + id + "", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", "Too many records will be returned, please try to minimize your selection and try again." + ex);
            }
            return View(dt);
        }
        public ActionResult progress(int? id)
        {

            Session["ProjectId"] = id;
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("SELECT TOP(1000) p.ProjectName,st.SubTaskId,st.SubName, ass.Stage FROM[IdaDB].[dbo].[Employee] e,[dbo].[Assignment] ass,[dbo].[Project] p, [dbo].[SubTask] st with(nolock) where e.EmpId = ass.EmpId and st.SubTaskId = ass.SubTaskId and ass.ProjectId = p.ProjectId and ass.ProjectId =" + id + "", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", "Too many records will be returned, please try to minimize your selection and try again." + ex);
            }
            return View(dt);
        }
    }
}