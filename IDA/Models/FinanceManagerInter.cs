using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDA.Models
{
    public interface FinanceManagerInter
    {
        Task<DataTable> GetDataTable();
        Task<List<LogisticBooking>> MonthlyExpenses();
        Task<IEnumerable<Employee>> GetAllEmpSalary();
        void AddExpenditures(LogisticBooking logistic, Employee employee, Project project);
        Task<IEnumerable<Employee>> SelectEmployee();
        Task<IEnumerable<Project>> SelectProject();
        int CountNewEmployee();
        int CountNewProject();
        int CountProject_Req();
        int CountLogistics();
        int CountTotalNotifications();
    }
}
