using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IDA.Models;

namespace IDA.Controllers
{
    public class ProjectsController : Controller
    {
        private IdaDBEntities db = new IdaDBEntities();
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ToString();
            con = new SqlConnection(constring);
        }
        // GET: Projects
        public ActionResult Index()
        {
            List<Project> projects = db.Projects.ToList();
            List<Project> activeProjects = new List<Project>();
            foreach (var item in projects)
            {
                if (item.Status.Equals("Active"))
                {
                    activeProjects.Add(item);
                    
                }
            }
            return View(activeProjects);
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SqlDataReader dr = null;
            Client c = new Client();
            Project project = db.Projects.Find(id);

            int ID =Convert.ToInt32( project.ClientId);
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Client where ClientId=" +ID);
            cmd.Connection = con;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                c.ClientId = Convert.ToInt32(dr["ClientId"]);
                c.Name = dr["Name"].ToString();
                c.Surname = dr["Surname"].ToString();
                c.Company = dr["Company"].ToString();
                c.Email = dr["Email"].ToString();
                c.PhoneNo = dr["PhoneNo"].ToString();
                
            }
            dr.Close();
            con.Close();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }
        public ActionResult projectRequests()
        {
            return View(db.Projects.ToList());
        }
        public ActionResult rejectedProjects()
        {
            return View(db.Projects.ToList());
        }
        // GET: Projects/Createc = new 
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectName,Date_Started,Date_Concluded,ClientId,EmpId")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,ProjectName,Date_Started,Date_Concluded,ClientId,EmpId")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Progress(int? id)
        {
            Session["ProId"] = id;
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("SELECT TOP(1000) e.[Name],e.[Surname], st.SubName,p.ProjectName,st.SubTaskId, ass.Stage FROM[IdaDB].[dbo].[Employee] e,[dbo].[Assignment] ass,[dbo].[Project] p, [dbo].[SubTask] st with(nolock) where e.EmpId = ass.EmpId and st.SubTaskId = ass.SubTaskId and ass.ProjectId = p.ProjectId and ass.ProjectId =" + id+"", con);

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
        public ActionResult Tasks(int? id)
        {
       
         
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select Project.ProjectName, SubTask.SubName, Employee.[Name],Employee.Surname  from Project,Employee,Assignment,SubTask where Assignment.ProjectId = Project.ProjectId and Assignment.EmpId = Employee.EmpId and SubTask.SubTaskId = Assignment.SubTaskId and Assignment.ProjectId =" + id + "", con);

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
        public ActionResult TimeSheets(int id)
        {
            Session["ProId"] = id;

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
        public ActionResult PastProjects(int? id)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("SELECT TOP(1000) [Name],[Surname], st.SubName, ass.Stage FROM[IdaDB].[dbo].[Employee] e,[dbo].[Assignment] ass, [dbo].[SubTask] st with(nolock) where e.EmpId = ass.EmpId and st.SubTaskId = ass.SubTaskId and ass.ProjectId = " + id + "", con);

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
        public ActionResult AddToAllProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddToAllProject(FormCollection f, HttpPostedFileBase file)
        {
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
                    p.ProjectType = "Project Bid";
                    p.AdvertDate = Convert.ToDateTime( f["AdvertDate"]);
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


        [HttpPost]
        public ActionResult InsertStage(int? id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    String query = "Select * from Employee where Username=" + Session["Surname"] ;
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand(query, con);

                    da.Fill(dt);
                    if (!(dt.Rows.Contains(null)))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            String position = row["Position"].ToString();

                            if (position.Equals("CEO"))
                            {
                                return RedirectToAction("Dashboard", "Employees");
                            }


                        }
                    }

                    else
                    {
                        return RedirectToAction("Dashboard", "Index");
                    }

                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }
            return View();
        }
        public ActionResult insertdata(string ProjectName, DateTime Date_Started, DateTime Date_Concluded, int ClientId, int EmpId)
        {
            string msg = "";
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("insert into Project values(@ProjectName, @Date_Started,@Date_Concluded,@ClientId,@EmpId)", con);
            cmd.Parameters.AddWithValue("@ProjectName", ProjectName);
            cmd.Parameters.AddWithValue("@Date_Started", Date_Started);
            cmd.Parameters.AddWithValue("@Date_Concluded", Date_Concluded);
            cmd.Parameters.AddWithValue("@ClientId", ClientId);
            cmd.Parameters.AddWithValue("@EmpId", EmpId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i == 1)
            {
                msg = "true";
                Session["message"] = msg;
            }
            else
            {
                msg = "false";
                Session["message"] = msg;
            }

            return View();

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ProjectAssigne()
        {

            var GetUser = db.Employees.Where(c => c.Position!=("CEO") && !db.AssignedPMs.Select(b => b.EmpId).Contains(c.EmpId));
            ViewBag.ChooseUser = new SelectList(GetUser, "EmpId", "Email");


            var ProjectsNames = db.Projects.Where(c => !db.AssignedPMs.Select(b => b.ProjectId).Contains(c.ProjectId));
            ViewBag.ProjectName = new SelectList(ProjectsNames, "ProjectId", "ProjectName");

            return View();
        }
        [HttpPost]
        public ActionResult ProjectAssigne(User user, Project project, Employee employee, Models.Task task, SubTask sub, Assignment assignment, AssignedPM pM)
        {

            try
            {

                var GetUser = db.Employees.Where(c => c.Position.Equals("CEO") && !db.AssignedPMs.Select(b => b.EmpId).Contains(c.EmpId));
                ViewBag.ChooseUser = new SelectList(GetUser, "EmpId", "Email");

                var ProjectsNames = db.Projects.Where(c => !db.AssignedPMs.Select(b => b.ProjectId).Contains(c.ProjectId));
                ViewBag.ProjectName = new SelectList(ProjectsNames, "ProjectId", "ProjectName");


                var UserId = db.Employees.Find(employee.EmpId);
                var ProId = db.Projects.Find(project.ProjectId);

                if (UserId == null || ProId == null)
                {
                    ViewBag.ErrorBox = "All field are required!!!";
                }
                else
                {

                    pM.DateAssigned = DateTime.Now;
                    pM.EmpPosition = "Project Manager";
                    db.AssignedPMs.Add(pM);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Assigned Succefully!!!";
                }
            }
            catch
            {

            }
            return View(assignment);
        }
       
        public ActionResult TaskDetails(int? id)
        {
            
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                    SqlCommand cmd = new SqlCommand("select p.ProjectName,st.SubName,e.[Name], e.Surname, ass.DateAssigned from Employee e, SubTask st,Assignment ass,Project p where p.ProjectId = ass.ProjectId and e.EmpId = ass.EmpId and st.SubTaskId = ass.SubTaskId and ass.SubTaskId= " + id + "", con);

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
