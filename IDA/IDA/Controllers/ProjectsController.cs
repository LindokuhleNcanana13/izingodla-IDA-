using System;
using System.Collections.Generic;
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
    public class ProjectsController : Controller
    {
        private IdaDBEntities db = new IdaDBEntities();

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
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

        // GET: Projects/Create
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
        public ActionResult Progress()
        {
            IdaDBEntities sample = new IdaDBEntities();
            var getEmpList = sample.Employees.ToList();

            SelectList list = new SelectList(getEmpList,"Name", "Surname");
            ViewBag.EmployeeList = list;

            return View(db.Tasks.ToList());

        }
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
            catch (Exception e)
            {
                return View("User no clear");
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
    }
}
