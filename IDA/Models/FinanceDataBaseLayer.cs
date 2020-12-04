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
        public void AddExpenditures(LogisticBooking logistic, Employee employee, Project project)
        {
            logistic.BookingDate = DateTime.Now;
            db.LogisticBookings.Add(logistic);
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
                    string query = "select Employee.Email,Project.ProjectName,Vehicle.VehicleName,Vehicle.BrandName,LogisticBooking.salary from Employee, Project, LogisticBooking, Vehicle where Employee.EmpId = LogisticBooking.EmpId and Project.ProjectId = LogisticBooking.ProjectId and Vehicle.VehicleId = LogisticBooking.VehicleId";
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
        public async Task<IEnumerable<Employee>> GetAllEmpSalary()
        {
            var EmpSalary = db.Employees.Distinct();
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
        
    }
}