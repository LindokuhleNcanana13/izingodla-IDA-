using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using IDA.Models;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;
using System.IO;



namespace IDA.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly FinanceManagerInter FinanceAccesser = new FinanceDataBaseLayer();
        private IdaDBEntities db = new IdaDBEntities();
        EmployeeDBHandler dbhandle = new EmployeeDBHandler();
        DownloadFile df = new DownloadFile();
        [HttpGet]
        public ActionResult GetEmployee()
        {
           
            ModelState.Clear();
            return View(dbhandle.GetEmployee());
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }
        public void DisplayBags()
        {
            ViewData["GetProject"] = FinanceAccesser.CountNewProject();
            ViewData["GetReq"] = FinanceAccesser.CountProject_Req();
            ViewData["GetLog"] = FinanceAccesser.CountLogistics();
            ViewData["GetTotalNotifications"] = FinanceAccesser.CountTotalNotifications();
            ViewData["GetNotify"] = FinanceAccesser.CountNewEmployee();

        }
        public ActionResult DashBoard()
        {
            try
            {
                DisplayBags();
                if (Session["Position"].Equals("CEO"))
                {

                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
        public ActionResult ApprovedLeaves()
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    return View(db.Leaves.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
        public ActionResult DeclinedLeaves()
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    return View(db.Leaves.ToList());
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
            
        }
        public ActionResult AdminProfile()
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    using (SqlConnection con = new SqlConnection())
                    {
                        con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                        SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", con);
                        cmd.CommandType = CommandType.Text;
                        con.Open();

                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            Session["Name"] = rdr["Name"].ToString();
                            Session["Surname"] = rdr["Surname"].ToString();
                            Session["Email"] = rdr["Email"].ToString();
                            Session["Address"] = rdr["Address"].ToString();
                            Session["PhoneNo"] = Convert.ToInt32(rdr["PhoneNo"]);
                            Session["IDNumber"] = Convert.ToInt32(rdr["IDNumber"]);
                            Session["Position"] = rdr["Position"].ToString();
                        }
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }
           
        }
        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["Position"].Equals("CEO"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Employee employee = db.Employees.Find(id);
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    if (ModelState.IsValid)
                    {
                        var isEmailAlreadyExists = db.Employees.Any(x => x.Email == employee.Email);
                        var isUserEmailAlreadyExists = db.Users.Any(x => x.Username == employee.Email);
                        if (isEmailAlreadyExists)
                        {
                            ViewBag.MessageError = "User with this email already exists!!";
                            return View(employee);
                        }
                        if (isUserEmailAlreadyExists)
                        {
                            ViewBag.MessageError = "User with this email already exists!!";
                            return View(employee);
                        }
                        EmployeeDBHandler sdb = new EmployeeDBHandler();
                        Random rnd = new Random();
                        int myRandomNo = rnd.Next(10000000, 99999999);
                        int year = employee.HireDate.Year;
                        string initials = employee.Name.Substring(0,1);
                        string surname = employee.Name.Substring(0, 1);
                        String EmployeeNumber = ("Izi" + year + initials + surname + myRandomNo);
                        employee.EmployeeNumber = EmployeeNumber;

                        if (sdb.AddEmployee(employee))
                        {

                            ViewBag.Message = "Employee Details Added Successfully";
                            ModelState.Clear();

                        }
                    }
                    else {
                        return View();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch
            {
                return View();
            }
        }

        
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    return View(employee);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
            
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpId,Name,Surname,Email,PhoneNo,Address,Gender,IDNumber,Salary,Position,HireDate")] Employee employee)
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(employee);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Employee employee = db.Employees.Find(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    return View(employee);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (Session["Position"].Equals("CEO"))
                {

                    Employee employee = db.Employees.Find(id);
                    User user = new User();
                    var u = db.Users.ToList();
                    foreach (var item in u)
                    {
                        if (item.EmpId == id)
                        {
                            db.Users.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    db.Employees.Remove(employee);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        public ActionResult EmpPerfomance(Employee employee, int id)
        {
         
            Session["eId"] = id;
            DataTable dt = new DataTable();
            try
            {
                if (Session["Position"].Equals("CEO"))
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
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", "Too many records will be returned, please try to minimize your selection and try again." + ex);
            }
            return View(dt);

        }
        public ActionResult newRequests()
        {
            if(Session["Position"].Equals("CEO") || Session["Position"].Equals(null))
            {
                return View(db.Projects.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public ActionResult AddProject([Bind(Include = "ProjectId,ProjectName,Date_Started,Date_Concluded,ClientId,EmpId")] Project project)
        {
            if (Session["Position"].Equals("CEO"))
            {
                Project p = new Project();
                db.Projects.Add(p);
                db.SaveChanges();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        int DownloadId = 0;
        public ActionResult ProjectDetails(int? id)
        {
           
                Session["ProjId"] = id;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Project pr = db.Projects.Find(id);
                Session["ProjectName"] = pr.ProjectName;
                Session["Path"] = pr.FilePath;
                string  f = pr.FilePath;
                Session["FileName"] = Path.GetFileName(f);
                DownloadId = pr.ProjectId;
                if (pr == null)
                {
                    return HttpNotFound();
                }
                return View(pr);
          
        }
        [HttpPost] 
        public ActionResult ProjectDetails(Project project)
        {

            Session["Path"] = project.FilePath;
            if (Session["Position"].Equals("CEO"))
            {
                if (ModelState.IsValid)
                {
                    if (project.Status.Equals("Active"))
                    {
                        project.Feedback = "Approved";
                        project.Date_Started = DateTime.Now;
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ProjectAssigne", "Projects");
                    }
                    else
                    {
                        Session["proId"] = project.ProjectId;
                        Session["Status"] = "Declined";
                        return RedirectToAction("DeclinedProject", "Employees", new { id = Convert.ToInt32(Session["ProjId"]) });


                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
           
            return View();
        }

        DownloadFile obj;
        public EmployeesController()
        {
            obj = new DownloadFile();
        }
        public ActionResult Download(int? DownloadId,Project project)
        {
            string contentType = string.Empty;
            
            if (Session["Position"].Equals("CEO"))
            {
                int CurrentFileID = Convert.ToInt32(Session["ProjId"]);
                var filesCol = obj.GetFiles();
                String CurrentFileName = "";
                //string CurrentFileName = (from f in filesCol
                //                          where f.ProjectId == CurrentFileID
                //                          select f.FilePath).First();
                foreach (var item in filesCol)
                {
                    if (item.FilePath.Equals(Session["Path"]))
                    {
                        CurrentFileName = item.FilePath;
                    }
                     
                    if (CurrentFileName.Contains(".pdf"))
                    {
                        contentType = "application/pdf";
                    }

                    else if (CurrentFileName.Contains(".docx"))
                    {
                        contentType = "application/docx";
                    }
                    
                }
                String fileName = Path.GetFileName(CurrentFileName);
                return File(CurrentFileName, contentType, fileName);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpGet]
        public ActionResult GetLeaves()
        {
            if (Session["Position"].Equals("CEO"))
            {
                return View(db.Leaves.ToList());
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult EditLeave(int? id)
        {
            if (Session["Position"].Equals("CEO"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Leave lv = db.Leaves.Find(id);
                if (lv == null)
                {

                    return HttpNotFound();
                }
                return View(lv);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLeave([Bind(Include = "LeaveId,AppDate,Duration,Status,DateApproved,EmpId,ApplicantName,ReasonForApplying,FromDate,ToDate,NoOfDays,Comment,FeedMessage")] Leave leave)
        {
            if (Session["Position"].Equals("CEO"))
            {


                if (ModelState.IsValid)
                {
                    leave.DateApproved = DateTime.Now;
                    db.Entry(leave).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = "Leave Updated Successfully";

                    return RedirectToAction("GetLeaves");
                }

                return View(leave);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        public ActionResult Reports(string ReportType)
        {
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/EmployeeReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "ProjectDataSet";
            reportDataSource.Value = db.Projects.ToList();
            localreport.DataSources.Add(reportDataSource);

            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            if (reportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }
            else if (reportType == "Word")
            {
                fileNameExtension = "docx";
            }
            else if (reportType == "PDF")
            {
                fileNameExtension = "pdf";
            }
            else
            {
                fileNameExtension = "jpg"; 
            }

            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;

            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            Response.AddHeader("Content-disposition","attachment:fileName = project_report."+fileNameExtension);
            return File(renderedByte,fileNameExtension);
            
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
            int random;
            // int EmpId = Convert.ToInt32(Session["EmpId"]);
            if (ModelState.IsValid)
            {
                Random rand = new Random();
                rand.Next();
                random = rand.Next();

                IdaDBEntities db = new IdaDBEntities();
                user.MeetingNo = random.ToString();
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
                    try
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
                        smtp.Port = 8889;
                        smtp.Send(mm);
                    }
                    catch (SmtpException)
                    {
                        ViewBag.ErrorMessage = "Server Error";
                    }
                }

                
            }
        }
    
        public ActionResult Assign()
        {
             
            Models.Task t = new Models.Task();
            var GetUser = db.Employees.ToList();
            ViewBag.ChooseUser = new SelectList(GetUser, "EmpId", "Email");


            var TaskDescription = db.Tasks.ToList();
            ViewBag.TaskName = new SelectList(TaskDescription, "TaskId", "TaskDescription");
            int Id = t.TaskId;


            var SubTaskId = db.SubTasks.ToList();
            List<SubTask> st = new List<SubTask>();
            foreach (var item in SubTaskId)
            {
                
                    st.Add(item);
                    ViewBag.SubTaskName = new SelectList(st, "SubTaskId", "SubName");
                
            }
            var ProjectsNames = db.Projects.ToList();
            List<Project> pp = new List<Project>();
            ViewBag.ProjectName = new SelectList(ProjectsNames, "ProjectId", "ProjectName");
            //foreach (var item in ProjectsNames)
            //{
            //    if (item.Status.Equals("Pending"))
            //    {
            //        pp.Add(item);
            //        ViewBag.ProjectName = new SelectList(pp, "ProjectId", "ProjectName");
            //    }
            //}
           


            return View();
        }
        [HttpPost]
        public ActionResult Assign(User user, Project project, Employee employee, Models.Task task, SubTask sub, Assignment assignment)
        {
            try
            {
                var GetUser = db.Employees.ToList();               
                ViewBag.ChooseUser = new SelectList(GetUser, "EmpId", "Email");


                var TaskDescription = db.Tasks.ToList();
                ViewBag.TaskName = new SelectList(TaskDescription, "TaskId", "TaskDescription");


                var SubTaskId = db.SubTasks.ToList();
                ViewBag.SubTaskName = new SelectList(SubTaskId, "SubTaskId", "SubName");


                var ProjectsNames = db.Projects.ToList();
                ViewBag.ProjectName = new SelectList(ProjectsNames, "ProjectId", "ProjectName");


                var UserId = db.Employees.Find(employee.EmpId);
                var ProId = db.Projects.Find(project.ProjectId);
                var taskid = db.Tasks.Find(task.TaskId);
                var subid = db.SubTasks.Find(sub.SubTaskId);

                if (UserId == null || ProId == null || taskid == null || subid == null)
                {
                    ViewBag.ErrorBox = "All field are required!!!";
                }
                else
                {
                   
                    assignment.DateAssigned = DateTime.Now;
                    assignment.Stage = 1;
                    db.Assignments.Add(assignment);
                    db.SaveChanges();
                    ViewBag.SucessMsg = "Assigned Succefully!!!";
                }
            }
            catch
            {

            }
            return View(assignment);
        }
        public ActionResult Bid()
        {
            List<Project> bid = db.Projects.ToList();
            List<Project> bd = new List<Project>();
            foreach (var item in bid)
            {
                if (item.ProjectType.Equals("Project Bid") && item.Status.Equals("Pending"))
                {
                    bd.Add(item);
                }
            }
            return View(bd);
        }
        public ActionResult ProjectReport(int? id)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select Employee.Name,Employee.Surname,ProjectReport.[Description],ProjectReport.ReportDate,Project.ProjectName from ProjectReport,Employee,Project where Employee.EmpId = [ProjectReport].EmpId and Project.ProjectId = ProjectReport.ProjectId and ProjectReport.ProjectId =" + id + "", con);

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
        public ActionResult DeclinedProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project pr = db.Projects.Find(id);

            if (pr == null)
            {
                return HttpNotFound();
            }
            return View(pr);

        }
        [HttpPost]
        public ActionResult DeclinedProject(Project pr,int id)
        {
            String feed = pr.Feedback;
                if (ModelState.IsValid)
                {
                    if (Session["Status"].Equals("Declined"))
                    {
                       pr = db.Projects.Find(Convert.ToInt32(Session["proId"]));
                    if (pr != null)
                    {
                        pr.Status = "Declined";
                        pr.Feedback = feed;
                        db.Entry(pr).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("rejectedProjects", "Projects");
                    }
                       
                    }
                }

            return View();
        }
        public ActionResult changePassword()
        {
       
            
            return View();
            
        }

        [HttpPost]
        public ActionResult changePassword([Bind(Include = "UserId,Username,Password,EmpId,ClientId,AdminId")] User user,FormCollection f)
        {
            var obj1 = db.Users.Where(x => x.Password.Equals(user.Password) && x.EmpId != 0).FirstOrDefault();
            var id = db.Users.Where(cx => cx.Password == user.Password).Select(cx => cx.UserId).FirstOrDefault();
            try
            {
                if (obj1 == null)
                {
                    TempData["error"] = "Incorrect Password";
                }
                else
                {
                    if (f["newPassword"] == f["confirmPassword"])
                    {

                        user.AdminId = 0;
                        user.ClientId = 0;
                        user.EmpId = Convert.ToInt32(Session["EmpId"]);
                        user.UserId = id;
                        user.Username = Session["Email"].ToString();
                        user.Password = f["newPassword"];
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["success"] = "success";
                    }
                    else
                    {
                        ViewBag.error = "Passwords do not Match!";
                    }

                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }
            return View();
        }

       public ActionResult getfeedback()
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
        public ActionResult ClientDetails(int? id)
        {
            int ClientId = 0;
            id = Convert.ToInt32(Session["ProjId"]);
            var project = db.Projects.Find(id);

            if (project != null)
            {
               ClientId =Convert.ToInt32(project.ClientId);
               Session["ProjectType"] = project.ProjectType;
               
            }

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select Name,Surname,Company,Email from Client where ClientId =" + ClientId + "", con);

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
                    db.Feedbacks.Add(feed);
                    db.SaveChanges();
                    ViewBag.Message = "Feedback Message Submitted";
                }
            }
            catch (ArgumentNullException ex)
            {
                return View(ex.Message);
            }
            return View();
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
        public ActionResult ViewPDF()
        {
            return View();
        }
    }
}