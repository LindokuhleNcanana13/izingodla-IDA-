using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDA.Models;
using System.Data.SqlClient;
using System.Data;

namespace IDA.Controllers
{
    public class AccountController : Controller
    {
        IdaDBEntities db = new IdaDBEntities();
        LoginEntity dc = new LoginEntity();
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
            if(string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Username))
            {
                return View();
            }
            try
            {
                if (ModelState.IsValid)
                {

                    var isValid = dc.Users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();

                    if (isValid != null)
                    {
                        Session["name"] = user.Username;
                        // Session["UserId"] = isValid.UserId.Tostring();
                        return RedirectToAction("Dashboard", "Employees");
                        //try
                        //{
                        //    using (SqlConnection con = new SqlConnection())
                        //    {
                        //        String query = "Select * from Employee where Username="+user.Username;
                        //        con.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IdaDB;Integrated Security=True";

                        //        DataTable dt = new DataTable();
                        //        SqlDataAdapter da = new SqlDataAdapter();
                        //        da.SelectCommand = new SqlCommand(query,con);

                        //        da.Fill(dt);
                        //        if (!(dt.Rows.Contains(null)))
                        //        {
                        //            foreach (DataRow row in dt.Rows)
                        //            {
                        //                String position = row["Position"].ToString();

                        //                if (position.Equals("CEO"))
                        //                {
                        //                    RedirectToAction("Dashboard", "Employees");
                        //                }


                        //            }
                        //        }
                        //        else
                        //        {
                        //            return RedirectToAction("Dashboard", "Index");
                        //        }


                        //    }
                        //}
                        //catch(Exception e)
                        //{
                        //    ViewBag.Message = "User no clear";
                        //}

                    }
                    else
                    {
                         ModelState.AddModelError("", "Username and Password incorect");
                        return View();

                    }

                    //}
                }
            }
            catch
            {
                throw;
            }
            return View(user);

        }
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Client model)
        {
            User u = new User();

            var user = new IdaDBEntities();
            using (var context = new IdaDBEntities())
            {
                context.Clients.Add(model);
                u.Username = model.Email;
                u.Password = model.Password;
                u.ClientId = model.ClientId;
                context.Users.Add(u);
                context.SaveChanges();
                
            }
            return RedirectToAction("Login");
        }
    }

}