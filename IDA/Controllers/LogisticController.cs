using IDA.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace IDA.Controllers
{
    public class LogisticController : Controller
    {
        IdaDBEntities db = new IdaDBEntities();
        // GET: Logistic
        public ActionResult LogisticDashboard()
        {
            return View();
        }
        public ActionResult Profile()
        {
            return View();
        }
        public ActionResult manageLogistics()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand(" select e.[Name], e.Surname, p.ProjectName,l.ReasonForBooking,l.Status,l.LogisticBookingId,l.FromDate,l.ToDate,l.BookingDate,v.VehicleName from[IdaDB].[dbo].[Employee] e, [IdaDB].[dbo].[Project] p, [IdaDB].[dbo].[LogisticBooking] l,[IdaDB].[dbo].[Vehicle] v where e.EmpId = l.EmpId and p.ProjectId = l.ProjectId and v.VehicleId = l.VehicleId", con);

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
        public ActionResult getVehicles()
        {
            return View(db.Vehicles.ToList());
        }
        public ActionResult AddVehicle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddVehicle(Vehicle v)
        {
            if (ModelState.IsValid)
            {
                v.Status = "RoadWorthy";
                v.DateRegisterd = DateTime.Now;
                db.Vehicles.Add(v);
                db.SaveChanges();
                return RedirectToAction("getVehicles", "Logistic");
            }
            return View();
        }
        public ActionResult UpdateBooking(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LogisticBooking booking = db.LogisticBookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        [HttpPost]
        public ActionResult UpdateBooking(LogisticBooking l)
        {
            if (ModelState.IsValid)
            {
                if (!(l.Status.Equals("Approved") || l.Status.Equals("Declined")))
                {
                    ViewBag.Error = "Select Status";
                }

                else
                {
                    l.DateAttended = DateTime.Now;
                    db.Entry(l).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = "successful";
                }
            }
            return View(l);
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
                user.EmpId = null;
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
            return RedirectToAction("Meeting");
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
        public ActionResult BookedVehicles()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("Select v.VehicleName,v.Model , p.ProjectName,e.[Name],e.Surname,l.LogisticBookingId,l.BookingDate,l.ReasonForBooking,l.[Status] from LogisticBooking l, Employee e, Project p, Vehicle v  where p.ProjectId = l.ProjectId and e.EmpId = l.EmpId and v.VehicleId = l.VehicleId", con);

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
        public ActionResult ApprovedBookings()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("Select v.VehicleName,v.Model , p.ProjectName,e.[Name],e.Surname,l.DateAttended,l.LogisticBookingId,l.BookingDate,l.ReasonForBooking,l.[Status] from LogisticBooking l, Employee e, Project p, Vehicle v  where p.ProjectId = l.ProjectId and e.EmpId = l.EmpId and v.VehicleId = l.VehicleId", con);

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
        public ActionResult DeclinedBookings()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("Select v.VehicleName,v.Model , p.ProjectName,e.[Name],e.Surname,l.DateAttended,l.LogisticBookingId,l.BookingDate,l.ReasonForBooking,l.[Status] from LogisticBooking l, Employee e, Project p, Vehicle v  where p.ProjectId = l.ProjectId and e.EmpId = l.EmpId and v.VehicleId = l.VehicleId", con);

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
        public ActionResult LeaveList()
        {
            return View(db.Leaves.ToList());
        }
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
        public ActionResult transactions()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand(" select e.[Name],e.Surname,t.SlipPath,t.TransactionId,lb.ReasonForBooking, p.ProjectName,t.item,t.Price,t.DateRecorded from LogisticBooking lb, Employee e,Project p,[Transaction] t where p.ProjectId = t.ProjectId  and lb.EmpId = e.EmpId and t.LogisticBookId = lb.LogisticBookingId and p.ProjectId = lb.ProjectId", con);

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
        DownloadFile obj;
        public LogisticController()
        {
            obj = new DownloadFile();
        }
        public ActionResult Download(int? id, Transaction transaction)
        {
            string contentType = string.Empty;
            String path = null;
            var list = db.Transactions.ToList();
            foreach (var item in list)
            {
                if (item.TransactionId == id)
                {
                    path = item.SlipPath;
                }
            }
            int CurrentFileID = Convert.ToInt32(Session["ProjId"]);
            var filesCol = obj.GetSlip();
            String CurrentFileName = "";

            foreach (var item in filesCol)
            {
                if (item.SlipPath.Equals(path))
                {
                    CurrentFileName = item.SlipPath;
                }

                if (CurrentFileName.Contains(".pdf"))
                {
                    contentType = "application/pdf";
                }
                if (CurrentFileName.Contains(".txt"))
                {
                    contentType = "application/txt";
                }

                else if (CurrentFileName.Contains(".docx"))
                {
                    contentType = "application/docx";
                }

            }
            String fileName = Path.GetFileName(CurrentFileName);
            return File(CurrentFileName, contentType, fileName);


        }
        public ActionResult availableVehicle()
        {
            return View();
        }
        public ActionResult unroadworthyCars(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Vehicle vehicle = db.Vehicles.Find(id);
                if (vehicle == null)
                {
                    return HttpNotFound();
                }
                return View(vehicle);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult unroadworthyCars(Vehicle vehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(vehicle).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("getVehicles", "Logistic");
                }
                return View(vehicle);


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}