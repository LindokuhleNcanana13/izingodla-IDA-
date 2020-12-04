using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using IDA.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IDA.Controllers
{
    public class StaffController : Controller
    {
        private readonly FinanceManagerInter FinanceAccesser = new FinanceDataBaseLayer();
        IdaDBEntities db = new IdaDBEntities();
        public void DisplayBags()
        {
            ViewData["GetProject"] = FinanceAccesser.CountNewProject();
            ViewData["GetReq"] = FinanceAccesser.CountProject_Req();
            ViewData["GetLog"] = FinanceAccesser.CountLogistics();
            ViewData["GetTotalNotifications"] = FinanceAccesser.CountTotalNotifications();
            ViewData["GetNotify"] = FinanceAccesser.CountNewEmployee();

        }
        [HttpGet]
        public ActionResult StaffPortal(Employee employee, int? id)
        {

            DisplayBags();

            id = Convert.ToInt32(Session["EmpId"]);
            var pm = db.AssignedPMs.ToList();
            AssignedPM a = new AssignedPM();
            foreach (var item in pm)
            {
                if (item.EmpId.Equals(id))
                {
                    a.EmpId = item.EmpId;
                    Session["ProjectManager"] = "true";
                }
            }
            Session["Path"] = "/Staff/AuthenticatePM";

            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select distinct Project.ProjectName, Assignment.ProjectId from Project,Employee,Assignment where Assignment.ProjectId = Project.ProjectId and Assignment.EmpId =" + id + "", con);

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
        [HttpPost]
        public ActionResult StaffPortal(HttpPostedFileBase file)
        {
            List<Project> pro = db.Projects.ToList();

            if (!file.Equals(null))
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
            return View(pro);

        }
        public ActionResult GetProject(int? id)
        {
            id = Convert.ToInt32(Session["EmpId"]);
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select distinct Project.ProjectName, Assignment.ProjectId from Project,Employee,Assignment where Assignment.ProjectId = Project.ProjectId and Assignment.EmpId =" + id + "", con);

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
        public ActionResult StaffProfile()
        {
            return View();
        }
        public ActionResult Progress()
        {
            return View();
        }
        public ActionResult LeaveList()
        {
            return View(db.Leaves.ToList());
        }
        [HttpGet]
        public ActionResult LeaveApplication()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LeaveApplication([Bind(Include = "LeaveId,AppDate,Duration,Status,DateApproved,EmpId,ReasonForApplying,FromDate,ToDate,NoOfDays,Comment")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                leave.EmpId = Convert.ToInt32(Session["EmpId"]);
                leave.Status = "Pending";
                leave.AppDate = DateTime.Now;
                leave.ApplicantName = Session["Name"].ToString() + " " + Session["Surname"].ToString();
                db.Leaves.Add(leave);
                db.SaveChanges();
                return RedirectToAction("LeaveList");
            }
            return View(leave);
        }
        public ActionResult sendReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sendReport([Bind(Include = "ReportId,WorkDescription,HoursWorked,TargetedHours,EmpId")] Report report)
        {
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        EmployeeDBHandler sdb = new EmployeeDBHandler();
            //        if (sdb.AddReport(report))
            //        {
            //            ViewBag.Message = "Report Details Added Successfully";
            //            ModelState.Clear();

            //        }
            //    }
            //    return RedirectToAction("StaffPortal");
            //}
            //catch(Exception ex)
            //{
            //    return View(ex);
            //}

            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("StaffPortal");
            }
            return View(report);
        }

        public ActionResult Meeting()
        {
            return View();
        }
        public JsonResult GetMeetings()
        {
            using (IdaDBEntities dc = new IdaDBEntities())
            {
                var meetings = dc.Meetings.ToList();
                return new JsonResult { Data = meetings, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                user.EmpId = Convert.ToInt32(Session["EmpId"]);
                user.ClientId = null;
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
            return RedirectToAction("Meeting", "Staff");
        }

        public void SendEmail(Meeting user)
        {
            using (var ctx = new IdaDBEntities())
            {
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
                    body += "<br /><br />Subject: " + Subject;
                    body += "<br /><br />Start Time: " + StartDate;
                    body += "<br /><br />End Time: " + EndDate;
                    body += "<br /><br />Location: " + Description;
                    body += "<br /><br />  We would be awaiting your esteemed presence in the meeting. Please feel free to contact us anytime, if you require any change in the meeting schedule.";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Staff/Feedback", Request.Url.Scheme, Request.Url.Authority) + "'>Click here to report</a>";
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
        public ActionResult ViewTask(int? id)
        {
            Session["pId"] = id;
            int eId = Convert.ToInt32(Session["EmpId"]);
            SqlDataReader dr;
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("SELECT TOP(1000) [Name],[Surname], st.SubName, ass.Stage, ass.AssignmentId, ass.ProjectId FROM[IdaDB].[dbo].[Employee] e,[dbo].[Assignment] ass, [dbo].[SubTask] st with(nolock) where e.EmpId = " + eId + " and st.SubTaskId = ass.SubTaskId and ass.ProjectId = " + id + "", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();


                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Session["ProjectId"] = Convert.ToInt32(dr["ProjectId"]);
                        }
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
        public ActionResult EditTaskStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment ass = db.Assignments.Find(id);
            if (ass == null)
            {
                return HttpNotFound();
            }
            return View(ass);
        }
        [HttpPost]
        public ActionResult EditTaskStatus([Bind(Include = "AssignmentId,TaskId,SubTaskId,ProjectId,EmpId,DateAssigned,ScheduleTime,Stage")] Assignment ass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewTask", "Staff", new { id = Convert.ToInt32(Session["ProjectId"]) });
            }
            return View();
        }
        public ActionResult insertData()
        {
            return View();
        }
        [HttpPost]
        public ActionResult insertData(ProjectReport pr, int? id)
        {

            int EmpId = Convert.ToInt32(Session["EmpId"]);
            try
            {
                if (!pr.Description.Equals(null))
                {
                    pr.EmpId = EmpId;
                    pr.ReportDate = DateTime.Now;
                    pr.ProjectId = id;
                    db.ProjectReports.Add(pr);
                    db.SaveChanges();
                    ViewBag.Message = "Report Submitted";
                }
                else
                {
                    ViewBag.ErrorMessage = "Description is required!!";

                }
            }
            catch
            {

            }
            return View();
        }

        public ActionResult AuthenticatePM(int? id, AssignedPM pM)
        {
            DataTable dt = new DataTable();

            id = Convert.ToInt32(Session["EmpId"]);
            try
            {

                //var GetPM = db.AssignedPMs.SqlQuery("select e.[Name] , apm.EmpId from [IdaDB].[dbo].[Employee] e, [IdaDB].[dbo].[AssignedPM] apm where e.EmpId = apm.EmpId and apm.EmpId =" + id);

                var pm = db.AssignedPMs.ToList();
                AssignedPM a = new AssignedPM();
                foreach (var item in pm)
                {
                    if (item.EmpId.Equals(id))
                    {
                        a.EmpId = item.EmpId;
                    }

                }

                if (a.EmpId == id)
                {
                    return RedirectToAction("Dashboard", "Manager");
                }
                else
                {
                    TempData["AlertMessage"] = "You are not assigned as Project Manager to any project";
                    return RedirectToAction("StaffPortal", "Staff");
                }
            }

            catch (SqlException ex)
            {
                ModelState.AddModelError("", "Too many records will be returned, please try to minimize your selection and try again." + ex);
            }
            return View(dt);
        }
        public ActionResult BookVehicle()
        {
            var GetVehicle = db.Vehicles.Where(c => c.Status.Equals("RoadWorthy") && !db.LogisticBookings.Select(b => b.VehicleId).Contains(c.VehicleId));
            ViewBag.ChooseVehicle = new SelectList(GetVehicle, "VehicleId", "VehicleName");
            return View();
        }
        [HttpPost]
        public ActionResult BookVehicle(LogisticBooking lb)
        {
            try
            {
                var GetVehicle = db.Vehicles.Where(c => c.Status.Equals("RoadWorthy") && !db.LogisticBookings.Select(b => b.VehicleId).Contains(c.VehicleId));
                ViewBag.ChooseVehicle = new SelectList(GetVehicle, "VehicleId", "VehicleName");

                var vehicle = db.LogisticBookings.Find(lb.VehicleId);

                lb.Status = "Pending";
                lb.BookingDate = DateTime.Now;
                lb.EmpId = Convert.ToInt32(Session["EmpId"]);
                lb.ProjectId = Convert.ToInt32(Session["ProjectId"]);

                db.LogisticBookings.Add(lb);
                db.SaveChanges();
                TempData["Success"] = "Booked Successfully";
            }
            catch (Exception ex)
            {
                return View(ex);
            }

            return View();
        }
        public ActionResult checkBookings()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand(" select e.[Name], e.Surname, p.ProjectName,l.EmpId,l.Status,l.ReasonForBooking,l.LogisticBookingId,l.FromDate,l.ToDate,l.ProjectId,l.BookingDate,v.VehicleName from[IdaDB].[dbo].[Employee] e, [IdaDB].[dbo].[Project] p, [IdaDB].[dbo].[LogisticBooking] l,[IdaDB].[dbo].[Vehicle] v where e.EmpId = l.EmpId and p.ProjectId = l.ProjectId and v.VehicleId = l.VehicleId", con);

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
        public ActionResult travellCosts()
        {
            return View();
        }
        [HttpPost]
        public ActionResult travellCosts(FormCollection f,int? id, HttpPostedFileBase file)
        {
           

                Transaction transaction = new Transaction();
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path1 = Path.Combine(Server.MapPath("~/Files/Slips"), fileName);
                    file.SaveAs(path1);
                    transaction.SlipPath = path1;
                    transaction.ProjectId =Convert.ToInt32( Session["prId"]);
                    transaction.LogisticBookId = id;
                    transaction.DateRecorded = DateTime.Now;
                    transaction.item = f["item"];
                    transaction.Price = f["Price"];
                   
                    
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                        ViewBag.Message = "Submitted successfully";
                  
                }
            

            return View();
        }
        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponents NC = new NotificationComponents();
            var list = NC.GetPM(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Feedback(Feedback feed)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    feed.description = "";
                    db.Feedbacks.Add(feed);
                    db.SaveChanges();
                    ViewBag.message = "success!!";
                }
            }
            catch (ArgumentNullException ex)
            {
                return View(ex.Message);
            }
            return View(feed);
        }
        public ActionResult getFeedback()
        {
            return View(db.Feedbacks.ToList());
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
         
        public ActionResult ViewPDF()
        {
            return View();
        }
    }
}

