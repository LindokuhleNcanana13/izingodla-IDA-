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
    public class EmployeesController : Controller
    {
        private IdaDBEntities db = new IdaDBEntities();

        // GET: Employees
        public ActionResult Index()
        {
            return View(db.Employees.ToList());
        }

        public ActionResult DashBoard()
        {

            return View();
        }
        public ActionResult AdminProfile()
        {
            try
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
            catch (Exception e)
            {
                return View();
            }
           
        }
        // GET: Employees/Details/5
        public ActionResult Details(int? id)
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

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpId,Name,Surname,Email,PhoneNo,Address,Gender,IDNumber,Salary,Position,HireDate")] Employee employee)
        {
            User u = new User();
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                u.EmpId = employee.EmpId;
                u.Username = employee.Email;
                u.Password = employee.IDNumber;
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpId,Name,Surname,Email,PhoneNo,Address,Gender,IDNumber,Salary,Position,HireDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult EmpPerfomance()
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Project project = db.Projects.Find(id);
            //if (project == null)
            //{
            //    return HttpNotFound();
            //}

            return View(db.Projects.ToList());
            //else
            //{
            //    try
            //    {
            //        using (SqlConnection con = new SqlConnection())
            //        {
            //            String query = "Select * from Project where EmpId=" +id;
            //            con.ConnectionString = "Data Source=(localdb)MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

            //            DataTable dt = new DataTable();
            //            SqlDataAdapter da = new SqlDataAdapter();
            //            da.SelectCommand = new SqlCommand(query, con);

            //            da.Fill(dt);
            //            if (!(dt.Rows.Contains(null)))
            //            {
            //                Project project = new Project();
            //                List<Project> p = new List<Project>();

            //                foreach (DataRow row in dt.Rows)
            //                {
            //                    project.ProjectName = row["ProjectName"].ToString();

            //                    p.Add(project);

            //                }
            //                return View(p);
            //            }

            //            else
            //            {
            //                return RedirectToAction("Dashboard", "Index");
            //            }

            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        return View("User no clear");
            //    }

            //}
        }
        public ActionResult newRequests()
        {
            return View(db.Project_Request.ToList());
        }
        [HttpPost]
        public ActionResult AddProject([Bind(Include = "ProjectId,ProjectName,Date_Started,Date_Concluded,ClientId,EmpId")] Project project)
        {
            //Employee e = new Employee();
            //Client c = new Client();
            //Project_Request pr = new Project_Request();
            Project p = new Project();
            //p.ProjectName = pr.ProjectName;
            //p.Date_Started = DateTime.Now;
            //p.ClientId = c.ClientId;
            //p.EmpId = e.EmpId;
            db.Projects.Add(p);
            db.SaveChanges();
            return View();
        }
        public ActionResult ProjectDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project_Request pr = db.Project_Request.Find(id);
            Session["ProjectName"] = pr.ProjectName;

            if (pr == null)
            {
                return HttpNotFound();
            }
            return View(pr);
       
        }
        //public ActionResult Reports(string ReportType)
        //{
        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath("~/Reports/ProjectReport.rdlc");

        //    ReportDataSource reportDataSource = new ReportDataSource();
        //    reportDataSource.Name = "ProjectDataSet";
        //    reportDataSource.Value = db.Employee.ToList();
        //    localReport.DataSources.Add(reportDataSource);

        //    string reportType = ReportType;
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;

        //    if (reportType == "Excel")
        //    {
        //        fileNameExtension = "xlsx";
        //    }
        //    else if (reportType == "Word")
        //    {
        //        fileNameExtension = "docx";
        //    }
        //    else if (reportType == "PDF")
        //    {
        //        fileNameExtension = "pdf";
        //    }
        //    else
        //    {
        //        fileNameExtension = "jpg";
        //    }
        //    string[] Streams;
        //    Warning[] warnings;
        //    byte[] renderedByte;
        //    renderedByte = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out Streams, out warnings);
        //    Response.AddHeader("content-disposition", "attachment: filename= Employee_Report." + fileNameExtension);
        //    return File(renderedByte, fileNameExtension);

        //}
    }
}
