using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IDA.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IDA.Controllers
{
    public class FinanceController : Controller
    {
        IdaDBEntities db = new IdaDBEntities();
        private readonly FinanceManagerInter FinanceAccesser = new FinanceDataBaseLayer();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FinanceDashboard()
        {
            try
            {
                DisplayBags();
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllExpense()
        {
            try
            {
                DisplayBags();
                ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
                ViewBag.GetProject = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");
                var Expense = await FinanceAccesser.GetDataTable();
                return View(Expense);
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> MonthlyExpenses()
        {
            try
            {
                var GetExpense = await FinanceAccesser.MonthlyExpenses();
                return Json(GetExpense, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }
        public async Task<JsonResult> AddExpen(LogisticBooking logistic, Employee employee, Project project)
        {
            ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
            ViewBag.GetProject = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");
            FinanceAccesser.AddExpenditures(logistic, employee, project);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> MonthExpense()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> YearlyExpenses()
        {
            try
            {
                return View();
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<ActionResult> InvoiceDetails()
        {
            try
            {
                return View();
            }
            catch
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<ActionResult> SalaryDetails()
        {
            DisplayBags();
            var EmpSalary = await FinanceAccesser.GetAllEmpSalary();
            return View(EmpSalary);
        }
        public void DisplayBags()
        {
            ViewData["GetProject"] = FinanceAccesser.CountNewProject();
            ViewData["GetReq"] = FinanceAccesser.CountProject_Req();
            ViewData["GetLog"] = FinanceAccesser.CountLogistics();
            ViewData["GetTotalNotifications"] = FinanceAccesser.CountTotalNotifications();
            ViewData["GetNotify"] = FinanceAccesser.CountNewEmployee();

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
        public ActionResult LeaveList()
        {
            return View(db.Leaves.ToList());
        }
        public ActionResult Profile()
        {
            return View();
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

                //var apiKey = Environment.GetEnvironmentVariable("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");
                var apiKey = ("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");

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

    }
}