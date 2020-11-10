using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IDA.Models
{
    public class ProjectDBHandler
    {
        IdaDBEntities db = new IdaDBEntities();
   
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ToString();
            con = new SqlConnection(constring);
        }


        //public bool AddBid(Project allPro)
        //{
        //    connection();
        //    SqlCommand cmd = new SqlCommand("AddToAllProjects", con);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue("@AllProjId", allPro.AllProjId);
        //    cmd.Parameters.AddWithValue("@Name", allPro.Name);
        //    cmd.Parameters.AddWithValue("@Date", allPro.Date);
        //    cmd.Parameters.AddWithValue("@ProjectId", allPro.ProjectId);
        //    cmd.Parameters.AddWithValue("@Location", allPro.Location);
            

        //    con.Open();
        //    int i = cmd.ExecuteNonQuery();
        //    con.Close();

        //    if (i >= 1)
        //        return true;
        //    else
        //        return false;

        //}

    }
}