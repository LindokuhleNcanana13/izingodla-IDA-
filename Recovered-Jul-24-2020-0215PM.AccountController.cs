using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDA.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace IDA.Controllers
{
    public class AccountController : Controller
    {
        IdaDBEntities db = new IdaDBEntities();
        LoginEntity dc = new LoginEntity();
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ToString();
            con = new SqlConnection(constring);
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            SqlDataReader dr = null;
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Username))
            {
                return View();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var obj1 = db.Users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password) && x.EmpId != 0).FirstOrDefault();
                    var obj2 = db.Users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password) && x.ClientId != 0).FirstOrDefault();
                    var obj3 = db.Users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password) && x.AdminId != 0).FirstOrDefault();

                    if (obj1 != null)
                    {
                        connection();
                            con.Open();
                            SqlCommand cmd = new SqlCommand("Select * from Employee where EmpId=" + obj1.EmpId);
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                Session["Name"] = dr["Name"].ToString();
                                Session["Surname"] = dr["Surname"].ToString();
                                Session["Address"] = dr["Address"].ToString();
                                Session["Email"] = dr["Email"].ToString();
                                Session["PhoneNo"] = dr["PhoneNo"].ToString();
                                Session["Gender"] = dr["Gender"].ToString();
                                Session["Position"] = dr["Position"].ToString();
                                Session["IDNumber"] = dr["IDNumber"].ToString();
                                Session["Salary"] = dr["Salary"];
                                Session["HireDate"] = Convert.ToDateTime( dr["HireDate"]);
                                
                            }
                        dr.Close();
                        con.Close();
                        return RedirectToAction("StaffPortal", "Staff");
                        
                    }
                    
                    if (obj2 != null)
                    {
                        connection();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select * from Client where ClientId=" + obj2.ClientId);
                        cmd.Connection = con;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Session["Name"] = dr["Name"].ToString();
                            Session["Surname"] = dr["Surname"].ToString();
                            Session["Company"] = dr["Company"].ToString();
                            Session["Email"] = dr["Email"].ToString();
                            Session["PhoneNo"] = dr["PhoneNo"].ToString();

                        }
                        dr.Close();
                        con.Close();
                        return RedirectToAction("Index", "Client");
                    }
                    
                    if (obj3 != null)
                    {
                        connection();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("Select * from Admin where AdminId=" + obj2.AdminId);
                        cmd.Connection = con;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            Session["Name"] = dr["Name"].ToString();
                            Session["Surname"] = dr["Surname"].ToString();
                            Session["Company"] = dr["Company"].ToString();
                            Session["Email"] = dr["Email"].ToString();
                            Session["PhoneNo"] = dr["PhoneNo"].ToString();

                        }
                        dr.Close();
                        con.Close();
                        //return RedirectToAction("Dashboard", "Employees");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username and Password incorect");
                    return View();
                }
                return View();
            }
            catch(Exception ex)
            {
               return ViewBag.Message = ex;
                throw;
            }
        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Client client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmployeeDBHandler sdb = new EmployeeDBHandler();
                    if (sdb.AddClient(client))
                    {

                        ViewBag.Message = "Client Details Added Successfully";
                        ModelState.Clear();

                    }
                }
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }
        
    }

}