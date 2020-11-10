using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IDA.Models;

namespace IDA
{
    public class NotificationComponents
    {
        public void RegisterNotification(DateTime currentTime)
        {

            string conString = ConfigurationManager.ConnectionStrings["IdaDBConnectionString"].ConnectionString;
            string sqlCommand = @"Select [EmpId],[AssignedPMID],[ProjectId] from [dbo].[AssignedPM] where DateAssigned > @DateAssigned";

            using (SqlConnection con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@DateAssigned", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;

                using (SqlDataReader reader = cmd.ExecuteReader())
                { 
                    
                }
            }
        }
        void sqlDep_OnChange(object sender,SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //sending notification to all clients
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");


                //re-register notification
                RegisterNotification(DateTime.Now);

            }
           // throw new NotImplementedException();
        }
        public List<AssignedPM> GetPM(DateTime afterDate)
        {
            using (IdaDBEntities db = new IdaDBEntities())
            {
                return db.AssignedPMs.Where(a=>a.DateAssigned > afterDate).OrderByDescending(a=>a.DateAssigned).ToList();
            }
        }
    }
}