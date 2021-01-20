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
using IDA.Models;

namespace IDA.Controllers
{
    public class ManagerController : Controller
    {
        private readonly FinanceManagerInter FinanceAccesser = new FinanceDataBaseLayer();
        IdaDBEntities db = new IdaDBEntities();
        // GET: Manager
        public void DisplayBags()
        {
            ViewData["GetProject"] = FinanceAccesser.CountNewProject();
            ViewData["GetReq"] = FinanceAccesser.CountProject_Req();
            ViewData["GetLog"] = FinanceAccesser.CountLogistics();
            ViewData["GetTotalNotifications"] = FinanceAccesser.CountTotalNotifications();
            ViewData["GetNotify"] = FinanceAccesser.CountNewEmployee();

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            DisplayBags();
            return View();
        }
        public ActionResult GetProjects(Employee employee, int? id)
        {
            id = Convert.ToInt32(Session["EmpId"]);
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select distinct Project.ProjectName, AssignedPM.ProjectId from Project,Employee,AssignedPM where AssignedPM.ProjectId = Project.ProjectId and AssignedPM.EmpId = Employee.EmpId and AssignedPM.EmpId =" + id + "", con);

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
        public ActionResult ViewTask(int? id)
        {
            int eId = Convert.ToInt32(Session["EmpId"]);
            Session["ProjectId"] = id;
            SqlDataReader dr;
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("SELECT TOP(1000) [Name],[Surname], st.SubName, ass.Stage, ass.AssignmentId, ass.ProjectId FROM[IdaDB].[dbo].[Employee] e,[dbo].[Assignment] ass, [dbo].[SubTask] st with(nolock) where e.EmpId = ass.EmpId and st.SubTaskId = ass.SubTaskId and ass.ProjectId = " + id + "", con);

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
        public ActionResult Assign()
        {
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
                else {

                    ViewBag.Message = "Description is required!!";
                }
            }
            catch
            {

            }
            return View();
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
                return RedirectToAction("ViewTask", "Manager", new { id = Convert.ToInt32(Session["ProjectId"])});
            }
            return View();
        }
        public ActionResult ManagerProfile()
        {
            return View();
        }
        public ActionResult Assigne_Active_projectTask(Task task, SubTask sub)
        {
            string UserEmail =Session["Email"].ToString();
            var GetEmployee = db.Employees.SqlQuery("Select * from Employee").ToList();
            ViewBag.ChooseEmp = new SelectList(GetEmployee, "EmpId", "Email");
            var GetTask = db.Tasks.ToList();
            ViewBag.ChooseTask = new SelectList(GetTask, "TaskId", "TaskDescription");
            var GetSubTask = db.SubTasks.Where(c => db.SubTasks.Select(b => b.SubTaskId).Contains(c.SubTaskId));
            ViewBag.ChoosesubTask = new SelectList(GetSubTask, "SubTaskId", "SubName");
            return View();
        }

        [HttpGet]
        public ActionResult Assigne_Active_projectTask()
        {
            string UserEmail = Session["Email"].ToString();
            var GetEmployee = db.Employees.Where(c => c.Position != ("CEO") && c.Position != ("logisticManager") && c.Position != ("financeManager") && c.Position != ("admin") && c.Email != (UserEmail)).ToList();
            ViewBag.ChooseEmp = new SelectList(GetEmployee, "EmpId", "Email");

            var GetTask = db.Tasks.ToList();
            ViewBag.ChooseTask = new SelectList(GetTask, "TaskId", "TaskDescription");

           

            var GetSubTask = db.SubTasks.Where(c => db.SubTasks.Select(b => b.SubTaskId).Contains(c.SubTaskId));
            ViewBag.ChoosesubTask = new SelectList(GetSubTask, "SubTaskId", "SubName");

            return View();
        }
        [HttpPost]
        public ActionResult Assigne_Active_projectTask(Project project, Employee employee, Task task, SubTask sub, Assignment assignment,int id)
        {
            string UserEmail = Convert.ToString(Session["Email"]);
            var GetEmployee = db.Employees.Where(c=>c.Position !=("CEO") && c.Email !=(UserEmail)).ToList();
            ViewBag.ChooseEmp = new SelectList(GetEmployee, "EmpId", "Email");

            var GetTask = db.Tasks.Where(c => db.SubTasks.Select(b => b.TaskId).Contains(c.TaskId));
            ViewBag.ChooseTask = new SelectList(GetTask, "TaskId", "TaskDescription");

            var GetSubTask = db.SubTasks.Where(c => db.SubTasks.Select(b => b.SubTaskId).Contains(c.SubTaskId));
            ViewBag.ChoosesubTask = new SelectList(GetSubTask, "SubTaskId", "SubName");

            var UserId = db.Employees.Find(employee.EmpId);
            var tasks = db.Tasks.Find(task.TaskId);
            var subtask = db.SubTasks.Find(sub.SubTaskId);
            assignment.ProjectId = id;
            assignment.Stage = 1;
            assignment.DateAssigned = System.DateTime.Now;
            assignment.ScheduleTime = DateTime.Now.ToString();
            db.Assignments.Add(assignment);
            db.SaveChanges();
            TempData["AlertMessage"] = "Task Assigned Successfull!";
            return View();
        }
        public ActionResult ViewPastproject(int? id)
        {
            if (Session["EmpId"] != null)
            {
                //var ViewProjectS = db.Projects.SqlQuery(" select * from Project where Status ='Active'").ToList();

                DataTable dt = new DataTable();
                try
                {
                    string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string query = "select distinct Project.ProjectName,AssignedPM.DateAssigned,AssignedPM.Concluded_Date,Employee.Email from Project, AssignedPM, Employee where AssignedPM.Concluded_Date < GETDATE() and Employee.EmpId = AssignedPM.EmpId and Project.ProjectId = AssignedPM.ProjectId";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                            {
                                sda.Fill(dt);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", "Too many records will be returned, please try to minimize your selection and try again." + ex);
                }
                return View(dt);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        public ActionResult Timesheets(int? id)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select Employee.[Name], Employee.Surname,ProjectReport.[Description],Project.ProjectName,ProjectReport.ReportDate from ProjectReport,Employee,Project where Employee.EmpId = [ProjectReport].EmpId and Project.ProjectId = ProjectReport.ProjectId and ProjectReport.ProjectId  = " + id + "", con);

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
        public ActionResult checkBookings()
        {
            Session["pId"] = Session["ProjectId"];
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
        public ActionResult getComments(int? id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select c.[Name], c.Surname, c.Company, p.ProjectName, cc.[Message] from[IdaDB].[dbo].[Client] c,[IdaDB].[dbo].[ClientComment] cc,[IdaDB].[dbo].[Project] p where c.ClientId = cc.ClientId and cc.ProjectId = p.ProjectId and cc.ProjectId = "+id, con);

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