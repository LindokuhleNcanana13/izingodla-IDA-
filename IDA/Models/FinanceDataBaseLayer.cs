using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using IDA.Models;


namespace IDA.Models
{
    public class FinanceDataBaseLayer: FinanceManagerInter
    {

        private IdaDBEntities db = new IdaDBEntities();
        public void AddExpenditures(Expenditure ex, Employee employee, Project project)
        {
            ex.DateAdded = DateTime.Now;
            db.Expenditures.Add(ex);
            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public void AddSalary(Salary sal, Employee employee)
        {
            sal.DatePaid = DateTime.Now;
            db.Salaries.Add(sal);
            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Employee>> SelectEmployee()
        {
            try
            {
                var GetEmployee = db.Employees.ToList();
                if (GetEmployee != null)
                {
                    return GetEmployee.AsQueryable();
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("No Employee Found")),
                        ReasonPhrase = "Employee Not Found"
                    };
                    throw new HttpResponseException(resp);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Project>> SelectProject()
        {
            var GetProject = db.Projects.ToList();

            List<Project> activeProjects = new List<Project>();
            foreach (var item in GetProject)
            {
                if (item.Status.Equals("Active"))
                {
                    activeProjects.Add(item);
                }
            }
            try
            {
                return GetProject.AsQueryable();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public async Task<DataTable> GetDataTable()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select e.[Name], e.Surname, p.ProjectName,ex.Description,ex.Amount, ex.DateAdded from Employee e, Project p, Expenditure ex where e.EmpId = ex.EmpId and ex.ProjectId = p.ProjectId";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        public async Task<List<LogisticBooking>> MonthlyExpenses()
        {
            ProcedureClass dbop = new ProcedureClass();
            DataSet ds = dbop.GetMonthlyExepnse();
            List<LogisticBooking> list = new List<LogisticBooking>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new LogisticBooking
                {
                    Salary = Convert.ToInt32(dr["Salary"]),
                    BookingDate = Convert.ToDateTime(dr["BookingDate"])
                });
            }
            return list;
        }
        public async Task<IEnumerable<Salary>> GetAllEmpSalary()
        {
            var EmpSalary = db.Salaries.Distinct();
            try
            {
                if (EmpSalary != null)
                {
                    return EmpSalary.AsQueryable();
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("No Employee Found")),
                        ReasonPhrase = "Employee Not Found"
                    };
                    throw new HttpResponseException(resp);
                }
            }
            catch
            {
                throw;
            }
        }
        public int CountNewEmployee()
        {
            int GetEmployee = db.Employees.Where(a => a.HireDate.Equals(DateTime.Today)).Count();
            try
            {
                return GetEmployee;
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
        }
        public int CountNewprojectManager()
        {
            int GetPm = db.AssignedPMs.Where(a => a.DateAssigned == (DateTime.Today)).Count();
            try
            {
                return GetPm;
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
        }
        public int CountLogistics()
        {
            int GetLogistic = db.LogisticBookings.Where(a => a.BookingDate == (DateTime.Today)).Count();
            try
            {
                return GetLogistic;
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
        }
        public int CountProject_Req()
        {
            int GetProject_Re = db.Projects.Where(a => a.DateRequested == (DateTime.Today)).Count();
            try
            {
                return GetProject_Re;
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
        }
        public int CountTotalNotifications()
        {
            try
            {
                int GetTotalNot = CountLogistics() + CountLogistics() + CountProject_Req() + CountNewprojectManager() + CountNewProject() + CountNewEmployee();

                return GetTotalNot;
            }
            catch
            {
                throw;
            }
        }
        public void AddInvoice(InvoiceTbl invoice)
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            invoice.InvoiceNumber = myRandomNo.ToString();
            db.InvoiceTbls.Add(invoice);

            try
            {
               
                    invoice.Invoicedate = DateTime.Now;
                    db.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public void AddInepReport(InepVariance variance)
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            variance.ReportNumber = myRandomNo;
            db.InepVariances.Add(variance);
            try
            {
              db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public int CountNewProject()
        {
            int GetProject = db.AssignedPMs.Where(a => a.DateAssigned == (DateTime.Today)).Count();
            try
            {
                return GetProject;
            }
            catch (DBConcurrencyException)
            {
                throw;
            }
        }
        public async Task<IEnumerable<InvoiceTbl>> DisplayeInvoice()
        {
            try
            {
                var GetInvoDetails = db.InvoiceTbls.ToList();
                if (GetInvoDetails != null)
                {
                    return GetInvoDetails.AsQueryable();
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent(string.Format("No Invioce Found")),
                        ReasonPhrase = "Invoice Not Found"
                    };
                    throw new HttpResponseException(resp);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
        public async Task<DataTable> Edit(int? id)
        {

            //var employee = db.InvoiceTbls.Find(id);
            //return employee;
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select distinct Employee.Email,InvoiceTbl.InvoiceNumber,InvoiceTbl.DateDue,InvoiceTbl.Invoicedate,InvoiceTbl.NoteMessage,InvoiceTbl.Amount from Employee,InvoiceTbl  where InvoiceTbl.InvoiceId = " + id + " and Employee.EmpId=InvoiceTbl.EmpId ";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

        }
        public string GetExpenditures(int? id)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select SUM(Amount) from Expenditure,Project where Expenditure.ProjectId = Project.ProjectId and Project.ProjectId =" + id;
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return query;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

        }
        public string GetTransaction(int? id)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = " select SUM(Price) from [Transaction],Project where [Transaction].ProjectId = Project.ProjectId and Project.ProjectId =" + id;
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return query;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

        }
        public async Task<DataTable> GetInepReport(int? id)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select distinct p.ProjectName,p.ProjectNumber,p.SourceOfFunding,p.ContractorType,p.ProjectT,p.Date_Started,p.Date_Concluded,iv.MunicipalityApproval,iv.AmountRecieved,iv.Pre_Engineering,iv.Design,iv.Procurement,iv.Construction,iv.CloseUp,iv.[Month],iv.MonthVarianceReason,iv.MonthCorrectiveAction, iv.[Year],iv.YearVarianceReason,iv.YearCorrectiveAction, iv.OtherComments,iv.ReportNumber from Project p, InepVariance iv where p.ProjectId = iv.ProjectId and p.ProjectId =" + id;
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

        }
        public async Task<DataTable> GetInfoRerpotPrinting(int id)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select distinct Employee.[Name],Employee.Email,Project.ProjectName,Project.Date_Concluded,Project.ContractNumber,Project.ProjectT,Project.ContractorType,Project.ProjectNumber,Project.ProjectType,Project.Province,Project.SourceOfFunding,Project.AdvertDate,InvoiceTbl.InvoiceNumber,InvoiceTbl.DateDue,InvoiceTbl.Invoicedate,InvoiceTbl.StatusIvo,InvoiceTbl.NoteMessage,InvoiceTbl.Amount,Project.ProjectId from Employee, AssignedPM, Project, InvoiceTbl where  Project.ProjectId = InvoiceTbl.ProjectId and Employee.EmpId = InvoiceTbl.EmpId and Employee.EmpId = AssignedPM.EmpId and Project.ProjectId = AssignedPM.ProjectId and AssignedPM.EmpPosition = 'Project Manager' and Project.ProjectId =" + id+" ";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        public async Task<DataTable> GetInfoRerpot()
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select distinct e.[Name],e.Email,p.ProjectName,p.Date_Concluded,p.ContractNumber,p.ProjectNumber,p.ProjectT,p.SourceOfFunding,p.AdvertDate,iv.InvoiceNumber, iv.DateDue,iv.Invoicedate,iv.StatusIvo,iv.NoteMessage,iv.Amount,p.ProjectId,iv.InvoiceNumber from[IdaDB].[dbo].[Employee] e,[IdaDB].[dbo].[Project] p,[IdaDB].[dbo].[AssignedPM] apm,[IdaDB].[dbo].[InvoiceTbl] iv where e.EmpId = apm.EmpId and p.ProjectId = apm.ProjectId and iv.ProjectId = apm.ProjectId and apm.EmpPosition = 'Project Manager'";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            return ds.Tables[0];
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
        public int GenerateInvoiceNo()
        {
            Random random = new Random();
            int InvoiceNum = random.Next();


            return InvoiceNum;
        }
        public double Total(int id)
        {
            int TotalAmount = Convert.ToInt32(GetTransaction(id) + GetExpenditures(id));
            return TotalAmount;
            
        }
       

    }
}