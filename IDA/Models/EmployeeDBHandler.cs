using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IDA.Models
{
    public class EmployeeDBHandler
    {
        IdaDBEntities db = new IdaDBEntities();
        private readonly FinanceManagerInter FinanceAccer = new FinanceDataBaseLayer();
       
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ToString();
            con = new SqlConnection(constring);
        }
        public bool AddEmployee(Employee employee)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewEmployeeRecord", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmpId", employee.EmpId);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Surname", employee.Surname);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@PhoneNo", employee.PhoneNo);
            cmd.Parameters.AddWithValue("@Address", employee.Address);
            cmd.Parameters.AddWithValue("@Gender", employee.Gender);
            cmd.Parameters.AddWithValue("@IDNumber", employee.IDNumber);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);
            cmd.Parameters.AddWithValue("@Position", employee.Position);
            cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;

        }
        public bool AddReport(Report report)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewReport", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ReportId", report.ReportId);
            cmd.Parameters.AddWithValue("@WorkDescription", report.WorkDescription);
            cmd.Parameters.AddWithValue("@HoursWorked", report.HoursWorked);
            cmd.Parameters.AddWithValue("@TargetedHours", report.TargetedHours);
            cmd.Parameters.AddWithValue("@EmpId",report.EmpId);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;

        }
        public bool AddClient(Client client)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewClientRecord", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ClientId",client.ClientId);
            cmd.Parameters.AddWithValue("@Name", client.Name);
            cmd.Parameters.AddWithValue("@Surname", client.Surname);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@PhoneNo", client.PhoneNo);
            cmd.Parameters.AddWithValue("@Company", client.Company);
            cmd.Parameters.AddWithValue("@Password", client.Password);
            cmd.Parameters.AddWithValue("@DateRegistered", client.DateRegistered);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;

        }
        public List<Employee> GetEmployee()
        {

            connection();
            List<Employee> EmployeeData = new List<Employee>();

            SqlCommand cmd = new SqlCommand("getEmployee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {

                EmployeeData.Add(
                    new Employee
                    {
                        EmpId = Convert.ToInt32(dr["EmpId"]),
                        Name = Convert.ToString(dr["Name"]),
                        Surname = Convert.ToString(dr["Surname"]),
                        Address = Convert.ToString(dr["Address"]),
                        Email = Convert.ToString(dr["Email"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        Position= Convert.ToString(dr["Position"]),
                        IDNumber = Convert.ToString(dr["IDNumber"]),
                        Salary = Convert.ToString(dr["Salary"]),
                        HireDate = Convert.ToDateTime(dr["HireDate"]),

                     });
            
            }

            return EmployeeData;
        }
        public List<Client> getClient()
        {

            connection();
            List<Client> ClientData = new List<Client>();

            SqlCommand cmd = new SqlCommand("getClient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {

                ClientData.Add(
                    new Client
                    {
                        ClientId = Convert.ToInt32(dr["ClientId"]),
                        Name = Convert.ToString(dr["Name"]),
                        Surname = Convert.ToString(dr["Surname"]),
                        Email = Convert.ToString(dr["Email"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Company = Convert.ToString(dr["Company"]),
                        Password = Convert.ToString(dr["Password"])
                    });

            }

            return ClientData;
        }
    }
}