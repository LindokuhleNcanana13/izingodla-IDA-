using System;
using System.Linq;
using System.Web.Mvc;
using IDA.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace IDA.Controllers
{
    public class AccountController : Controller
    {

        IdaDBEntities db = new IdaDBEntities();
       
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
       //Login Method
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
                            SqlCommand cmd = new SqlCommand("select * from Employee where EmpId =" + obj1.EmpId);
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                Session["EmpId"] = Convert.ToInt32(dr["EmpId"]);
                                Session["Name"] = dr["Name"].ToString();
                                Session["Surname"] = dr["Surname"].ToString();
                                Session["Address"] = dr["Address"].ToString();
                                Session["Email"] = dr["Email"].ToString();
                                Session["PhoneNo"] = dr["PhoneNo"].ToString();
                                Session["Gender"] = dr["Gender"].ToString();
                                Session["Position"] = dr["Position"].ToString();
                                Session["IDNumber"] = dr["IDNumber"].ToString();
                                Session["Salary"] = dr["Salary"];
                                Session["HireDate"] = Convert.ToDateTime(dr["HireDate"]);


                            }
                            dr.Close();
                            con.Close();

                        if (Session["Position"].Equals("CEO"))
                        {
                            return RedirectToAction("Dashboard", "Employees");
                        }
                        else if (Session["Position"].Equals("logisticManager"))
                        {
                            return RedirectToAction("LogisticDashboard", "Logistic");
                        }
                        else if (Session["Position"].Equals("financeManager"))
                        {
                            return RedirectToAction("FinanceDashboard", "Finance");
                        }
                        else
                        {
                            return RedirectToAction("StaffPortal", "Staff");
                        }

                        //}

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
                            Session["ClientId"] = Convert.ToInt32(dr["ClientId"]);
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
                else
                {
                    ViewBag.Message = "Username and Password incorrect";
                    return View();
                }
            }

                return View();
            }
                catch(Exception ex)
                {
                   return View(ex);
                    throw;
                }
        }
        //Client Sign Up
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Client user)
        {
           
            Session["Email"] = user.Email;
            Session["ClientId"] = user.ClientId;
            user.DateRegistered = DateTime.Now;
               if (ModelState.IsValid)
                {
                var isEmailAlreadyExists = db.Clients.Any(x => x.Email == user.Email);
                var isUserEmailAlreadyExists = db.Users.Any(x => x.Username == user.Email);
                if (isEmailAlreadyExists)
                {
                    ViewBag.MessageError = "User with this email already exists!!";
                    return View(user);
                }
                if (isUserEmailAlreadyExists)
                {
                    ViewBag.MessageError = "User with this email already exists!!";
                    return View(user);
                }


                EmployeeDBHandler sdb = new EmployeeDBHandler();
                    if (sdb.AddClient(user))
                    {
                        ModelState.Clear();
                    string message = string.Empty;
                    switch (user.ClientId)
                    {
                        case -1:
                            message = "";
                            break;
                        case -2:
                            message = "";
                            break;
                        default:
                            message = "Registration successful.\\nUser Id: " + user.ClientId.ToString();
                            SendActivationEmail(user);
                            break;
                    }
                }
                }
                    return RedirectToAction("OTP");
       
        }
        //Sending Email for Verification during sign up
        //public void SendEmail(Client user)
        //{
        //    using (var ctx = new IdaDBEntities())
        //    {
        //        Random random = new Random();

        //        var name = ctx.Database.SqlQuery<string>("select top 1 Name from Client where Email='"+user.Email+"'").SingleOrDefault();
        //        var surname = ctx.Database.SqlQuery<string>("SELECT top 1 Surname from Client where Email='" + user.Email + "'").SingleOrDefault();

        //        //var apiKey = Environment.GetEnvironmentVariable("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");
        //        var apiKey = "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";
        //        var client = new SendGridClient(apiKey);
        //        var from = new EmailAddress("Izingodla@gmail.com", "Izingodla Account Validation");
        //        var Emailsubject = "Email Adress Validation";
        //        var OTP = random.Next(1000, 9999);
        //        Session["OTP"] =Convert.ToInt32(OTP);
        //        foreach (var address in user.Email.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            var to = new EmailAddress(address);

        //            var plainTextContent = "Account Validation";
        //            string htmlContent = "Dear Sir/Ma’am,";
        //            htmlContent += "<br /><br />Thank you for your interest in working with us.";
        //            htmlContent += "<br /><br /> Name : " + name + "";
        //            htmlContent += "<br /> Surname : " + surname + "";
        //            htmlContent += "<br/> Please use the below One Time Pin for verification.";
        //            htmlContent += "<br /><br /> <b>OTP :"+ OTP+"</b>";
        //            htmlContent += "<br /><br />Thanks <br /> Izingodla Team";
        //            var msg = MailHelper.CreateSingleEmail(from, to, Emailsubject, plainTextContent, htmlContent);
        //            var response = client.SendEmailAsync(msg);
        //        }
        //    }
        //}
        public ActionResult Activation()
        {
            ViewBag.Message = "Invalid Activation code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                IdaDBEntities usersEntities = new IdaDBEntities();
                Tbl_UserActivation userActivation = usersEntities.Tbl_UserActivation.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                if (userActivation != null)
                {
                    usersEntities.Tbl_UserActivation.Remove(userActivation);
                    usersEntities.SaveChanges();
                    ViewBag.Message = "Activation successful.";
                }
            }

            return View();
        }
        private void SendActivationEmail(Client user)//send account activation email to user after registration
        {
            Guid activationCode = Guid.NewGuid();
            IdaDBEntities usersEntities = new IdaDBEntities();
            usersEntities.Tbl_UserActivation.Add(new Tbl_UserActivation
            {
                ClientId = user.ClientId,
                ActivationCode = activationCode
            }); ;
            usersEntities.SaveChanges();

            using (MailMessage mm = new MailMessage("Izingodla.IDA@gmail.com", user.Email))
            {
                mm.Subject = "Account Activation";
                string body = "Hello " + user.Name + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + string.Format("{0}://{1}/Account/Login/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";
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

                //To fix the smt error use link bellow
                //https://stackoverflow.com/questions/20906077/gmail-error-the-smtp-server-requires-a-secure-connection-or-the-client-was-not
                //mvc charts
                //https://canvasjs.com/asp-net-mvc-charts/
                //GmailAccount is => eservicesbookingsystem@gmail.com -- Password is => booking@123
            }
        }
        public ActionResult OTP()
        {
            return View();
        }
        [HttpPost]
        //One Time Pin for verification
        public ActionResult OTP(FormCollection form)
        {
            if (form != null)
            {
                int otp = Convert.ToInt32(form["title"].ToString());
              
                if (Session["OTP"].Equals(otp))
                {
                    ViewBag.Message = "Your Account has been verified!";
                    return RedirectToAction("Login");

                }
                else if (Session["OTP"].Equals(null))
                {
                    ViewBag.Message = "Session has expired";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Wrong Input";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Please enter your OTP";
                return View();
            }

        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(User user)
        {

            return View();
        }
        public ActionResult Logout()
        {
            // Code disables caching by browser. Hence the back browser button
            // grayed out and could not causes the Page_Load event to fire 
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login","Account");
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(User tb)//Forgot password 
        {

            if (ModelState.IsValid)
            {
                IdaDBEntities db = new IdaDBEntities();
                var user = (from userlist in db.Users
                            where userlist.Username == tb.Username
                            select new
                            {
                                userlist.UserId,
                                userlist.Username,
                                userlist.Password,

                            }).ToList();
                if (user.FirstOrDefault() == null)
                {
                    ViewBag.MessageError = "System cannot recognise your emai!!";
                }
                else if (user.FirstOrDefault() != null)
                {
                    Session["Username"] = user.FirstOrDefault().Username;
                    Session["UserId"] = user.FirstOrDefault().UserId;
                    Session["Password"] = user.FirstOrDefault().Password;
                    string message = string.Empty;
                    switch (tb.UserId)
                    {
                        case -1:
                            message = "";
                            break;
                        case -2:
                            message = "";
                            break;
                        default:
                            message = "Password sent successful.\\nUser Id: " + tb.UserId.ToString();
                            SendPasswordToEmail(tb);
                            break;

                    }
                    ViewBag.MessageSuccess = "Password sent!! Please check your email";
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email Provided");
                }
            }
            return View();
        }
        public ActionResult ActivationPassword()
        {
            ViewBag.Message = "Invalid Password.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                IdaDBEntities usersEntities = new IdaDBEntities();
                Tbl_UserActivation userActivation = usersEntities.Tbl_UserActivation.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                if (userActivation != null)
                {
                    usersEntities.Tbl_UserActivation.Remove(userActivation);
                    usersEntities.SaveChanges();
                    ViewBag.Message = "Password successful.";
                }
            }

            return View();
        }
        private void SendPasswordToEmail(User user)//Send password to user email address
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Username))
                {
                    ViewBag.Message = "Email required!!!!";
                }
                string pass = Session["Password"].ToString();
                long Password = long.Parse(pass);
                string Username = Session["Username"].ToString();

                Guid activationCode = Guid.NewGuid();
                IdaDBEntities usersEntities = new IdaDBEntities();
                usersEntities.Tbl_UserActivation.Add(new Tbl_UserActivation
                {
                    ClientId = user.UserId,
                    ActivationCode = activationCode
                });
                usersEntities.SaveChanges();
                using (MailMessage mm = new MailMessage("Izingodla.IDA@gmail.com", user.Username))
                {
                    mm.Subject = "Password Recovery";
                    string body = "Hello " + Username + ",";
                    body += "<br /><br />Your password is:" + Password + "";
                    body += "<br /><br />Please click the following link to login to your account";
                    body += "<br /><a href = '" + string.Format("{0}://{1}/Account/Login/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to login to your account.</a>";
                    body += "<br /><br />Thanks";
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
            catch (Exception ex)
            {
                ViewBag.Message = "Error" + ex.Message;
            }
        }
    }

}