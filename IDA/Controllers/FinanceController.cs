using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
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
        public async Task<JsonResult> AddExpen(Expenditure ex, Employee employee, Project project)
        {
            ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
            ViewBag.GetProject = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");
            FinanceAccesser.AddExpenditures(ex, employee, project);
            return Json("Added Successfully", JsonRequestBehavior.AllowGet);
        }
     
        public async Task<JsonResult> AddSalary(Salary ex, Employee employee)
        {
            ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
            double grossAmount =0;
            double netAmount =0;
            double overTimeAmount = 0;
            int Hours =Convert.ToInt32(ex.HoursWorked);
            double hourlyRate =Convert.ToInt32(ex.HourRate);
            int overTimeHours = Convert.ToInt32(ex.OverTimeHours);

            overTimeAmount = (hourlyRate * 1.5) * overTimeHours;
            grossAmount = (hourlyRate * Hours) + overTimeAmount;
            netAmount = grossAmount - (grossAmount * 0.1);

            ex.GrossAmount = grossAmount;
            ex.NetAmount = netAmount;
            
         
            FinanceAccesser.AddSalary(ex, employee);
            return Json("Added Successfully", JsonRequestBehavior.AllowGet);
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
    
        
        public async Task<ActionResult> SalaryDetails()
        {
            DisplayBags();
            ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select e.[Name], e.surname,s.DatePaid, s.HoursWorked, s.HourRate,s.OverTimeHours, s.GrossAmount, s.NetAmount from[IdaDB].[dbo].Employee e,  [IdaDB].[dbo].Salary s where e.EmpId = s.EmpId ", con);

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
        public ActionResult debtorsandcreditors()
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("Select * from Creditor", con);

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
        public ActionResult newCreditor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newCreditor(Creditor c)
        {
            if (ModelState.IsValid)
            {
                c.DateRecorded = DateTime.Now;
                db.Creditors.Add(c);
                db.SaveChanges();
                TempData["Success"] = "Success";
            }
            return View();
        }
        public ActionResult debtors()
        {
            return View(db.Debtors.ToList());
        }
        public ActionResult newDebtor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newDebtor(Debtor d)
        {
            if (ModelState.IsValid)
            {
                d.DateRecorded = DateTime.Now;
                db.Debtors.Add(d);
                db.SaveChanges();
                TempData["Success"] = "Success";
            }
            return View();
        }
        public ActionResult generateReports()
        {
            return View();
        }
        public async Task<ActionResult> inepPartReport()
        {
            ViewBag.ChooseProject = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");

            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("Select iv.ReportNumber,iv.[Month],iv.[Year],p.ProjectId,p.ProjectName from Project p, InepVariance iv where p.ProjectId = iv.ProjectId", con);

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
        public ActionResult inepPartReport(InepVariance variance)
        {
       
            return View();
        }
        public async Task<JsonResult> UserInvoice(InvoiceTbl invoice)
        {

            ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
            ViewBag.Project = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");
            FinanceAccesser.AddInvoice(invoice);
            return Json("Invoice Added Successfully", JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> NewInepReport(InepVariance variance)
        {
            ViewBag.ChooseProject = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");
            FinanceAccesser.AddInepReport(variance);
            return Json("Report Added Successfully", JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> ViewInvoice(int id)
        {
            var invoice = await FinanceAccesser.Edit(id);
            return View(invoice);
        }
        public async Task<ActionResult> ViewInepReport(int id)
        {

            Session["ProjectIdForReport"] = id;
            DateTime dt = DateTime.Now;
            int Month = Convert.ToInt32(dt.Month);
            if (Month == 1)
            {
                ViewBag.Month = "January";
            }
            else if (Month == 2)
            {
                ViewBag.Month = "February";
            }
            else if (Month == 3)
            {
                ViewBag.Month = "March";
            }
            else if (Month == 4)
            {
                ViewBag.Month = "April";
            }
            else if (Month == 5)
            {
                ViewBag.Month = "May";
            }
            else if (Month == 6)
            {
                ViewBag.Month = "June";
            }
            else if (Month == 7)
            {
                ViewBag.Month = "July";
            }
            else if (Month == 8)
            {
                ViewBag.Month = "August";
            }
            else if (Month == 9)
            {
                ViewBag.Month = "September";
            }
            else if (Month == 10)
            {
                ViewBag.Month = "October";
            }
            else if (Month == 11)
            {
                ViewBag.Month = "November";
            }
            else if (Month == 12)
            {
                ViewBag.Month = "December";
            }
            var report = await FinanceAccesser.GetInepReport(id);
            var Total = FinanceAccesser.GetTransaction(id) + FinanceAccesser.GetExpenditures(id); 
           

            var quey = db.Transactions.Where(x=> x.ProjectId == id).Sum(x =>x.Price);
            var quey1 = db.Expenditures.Where(x => x.ProjectId == id).Sum(x => x.Amount);
            ViewBag.TotalTran = quey + quey1;
            double Amount = 0;
            var value = db.InepVariances.SqlQuery("Select * from InepVariance where ReportNumber ="+Session["ReportNumber"]);
            List<InepVariance> iv = value.ToList();
            foreach (var item in iv)
            {
                if (item.AmountRecieved != null)
                {
                     Amount = Convert.ToDouble(item.AmountRecieved);
                }
            }
            
            ViewBag.Percentage = (ViewBag.TotalTran / Amount) * 100;
            getTransact(id);
            return View(report);
        }
        [HttpGet]
        public async Task<ActionResult> InvoiceDetails(string GenNumber)
        {
            try
            {
                ViewBag.ChooseEmp = new SelectList(await FinanceAccesser.SelectEmployee(), "EmpId", "Email");
                // ViewBag.GenerateInvoice = FinanceAccesser.GenerateInvoiceNo();
                ViewBag.ChooseProject = new SelectList(await FinanceAccesser.SelectProject(), "ProjectId", "ProjectName");
            


                var InvoDetails = await FinanceAccesser.DisplayeInvoice();
                return View(InvoDetails);
            }
            catch
            {
                throw;
            }
        }
        public async Task<ActionResult> Reports(int id)
        {
            var GenInforReport =await FinanceAccesser.GetInfoRerpotPrinting(id);
            var quey1 = db.Expenditures.Where(x => x.ProjectId == id).Sum(x => x.Amount);
            ViewBag.TotalTran =  quey1;
            return View(GenInforReport);
             
        }
        public async Task<ActionResult> ViewInfoReprt()
        {
            
            var GenInforReport = await FinanceAccesser.GetInfoRerpot();
            return View(GenInforReport);
        }
        public async Task<ActionResult> getTransact(int id)
        {
            id = Convert.ToInt32(Session["ProjectIdForReport"]);
            ViewBag.Total = Convert.ToInt32(FinanceAccesser.Total(id));
            ViewBag.TransTotal = db.Transactions.SqlQuery("select SUM(Price) from [Transaction],Project where [Transaction].ProjectId = Project.ProjectId and Project.ProjectId =" + id);
            ViewBag.ExpTotal = db.Expenditures.SqlQuery("select SUM(Amount) from Expenditure,Project where Expenditure.ProjectId = Project.ProjectId and Project.ProjectId =" + id);

            ViewBag.TotalTran = ViewBag.ExpTotal + ViewBag.TransTotal;

            //FinanceAccesser.Total(id);
      
            return View(ViewBag.Total);
        }
    }
}