using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IDA.Models
{
    public class ProcedureClass
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString);

        public DataSet GetMonthlyExepnse()
        {
            try
            {
                SqlCommand com = new SqlCommand("GetDataForMonthly", con);
                com.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch
            {
                throw;
            }
        }

    }
}