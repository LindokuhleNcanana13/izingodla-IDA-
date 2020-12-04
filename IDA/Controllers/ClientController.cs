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
using System.Net;
using System.Net.Mail;
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
                        var path1 = Path.Combine(Server.MapPath("~/MD/img"), fileName);
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
            int random;

            if (ModelState.IsValid)
            {
                Random rand = new Random();
                rand.Next();
                random = rand.Next();

                IdaDBEntities db = new IdaDBEntities();
                user.TDate = DateTime.Now;
                user.MeetingNo = random.ToString();
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


                var MeetingNo = ctx.Database.SqlQuery<string>("SELECT MeetingNo from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var StartDate = ctx.Database.SqlQuery<DateTime>("select StartDate from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var EndDate = ctx.Database.SqlQuery<DateTime>("SELECT EndDate from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var Subject = ctx.Database.SqlQuery<string>("SELECT Subject from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();
                var Description = ctx.Database.SqlQuery<string>("SELECT Description from Meeting where TDate=(SELECT MAX(TDate) FROM Meeting)").SingleOrDefault();

                if (string.IsNullOrWhiteSpace(user.MeetingId.ToString()))
                {
                    ViewBag.Message = "Email required!!!!";
                }

                Guid activationCode = Guid.NewGuid();
                IdaDBEntities usersEntities = new IdaDBEntities();
                usersEntities.Tbl_UserActivation.Add(new Tbl_UserActivation
                {
                    ClientId = user.MeetingId,
                    ActivationCode = activationCode
                });
                usersEntities.SaveChanges();
                using (MailMessage mm = new MailMessage("Izingodla.IDA@gmail.com", user.NewClientEmail))
                {
                    mm.Subject = "Meeting Invitation";
                    string body = "Dear Sir/Ma'am,";
                    body += "<br /><br />You are being invited to the meeting by Izingodla Engineering.";
                    body += "<br /><br /><b>Meeting ID: </b>" + MeetingNo;
                    body += "<br /><br />Subject: " + Subject;
                    body += "<br /><br />Start Time: " + StartDate;
                    body += "<br /><br />End Time: " + EndDate;
                    body += "<br /><br />Location: " + Description;
                    body += "<br /><br />Please click the following link if you won't be available";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Employees/Feedback/{2}", Request.Url.Scheme, Request.Url.Authority, null) + "'>Click here to give feedback.</a>";
                    body += "<br /><br /> We would be awaiting your esteemed presence in the meeting. Please feel free to contact us anytime, if you require any change in the meeting schedule.";
                    body += "<br /><br />Thanks <br /> Izingodla Team";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("Izingodla.IDA@gmail.com", "izingodla@123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
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
        public ActionResult getfeedback(int? id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("  select f.FeedbackId,f.Reason, f.MeetingId, f.Email from Meeting m, Feedback f  where m.MeetingNo = f.MeetingId and ClientId =" + id + "", con);

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
        public ActionResult deletenow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback meter = db.Feedbacks.Find(id);
            if (meter == null)
            {
                return HttpNotFound();
            }
            return View(meter);
        }

        // post: flat/delete/5
        [HttpPost, ActionName("deletenow")]
        [ValidateAntiForgeryToken]
        public ActionResult deleteconfirm(int id)
        {
            Feedback meter = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(meter);
            db.SaveChanges();
            return RedirectToAction("getfeedback");
        }
        public ActionResult MeetingDetails(int? id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select Subject,StartDate,EndDate,Description from Meeting where MeetingNo =" + id + "", con);

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
        [HttpGet]
        public ActionResult Comment(int? id)
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult Comment(int? id,ClientComment comment)
        {
            comment.ProjectId = Convert.ToInt32(id);
            comment.ClientId = Convert.ToInt32(Session["ClientId"]);
            
                db.ClientComments.Add(comment);
                db.SaveChanges();
                TempData["Message"] = "Success";
           
            return View();
        }
        public ActionResult ViewPDF()
        {
            return View();
        }
    }
}