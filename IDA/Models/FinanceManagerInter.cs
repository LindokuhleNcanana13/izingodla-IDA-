using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDA.Models;

namespace IDA.Models
{
    public interface FinanceManagerInter
    {
        Task<DataTable> GetInfoRerpotPrinting(int id);
        Task<DataTable> GetInfoRerpot();
        Task<DataTable> GetDataTable();
        Task<List<LogisticBooking>> MonthlyExpenses();
        Task<IEnumerable<Salary>> GetAllEmpSalary();
        void AddExpenditures(Expenditure ex, Employee employee, Project project);
        void AddSalary(Salary sal, Employee employee);
        Task<IEnumerable<Employee>> SelectEmployee();
        Task<IEnumerable<Project>> SelectProject();
        int CountNewEmployee();
        int CountNewProject();
        int CountProject_Req();
        int CountLogistics();
        int CountTotalNotifications();
        void AddInvoice(InvoiceTbl invoice);
        void AddInepReport(InepVariance variance);
        Task<DataTable> Edit(int? id);
        Task<DataTable> GetInepReport(int? id);
        string GetExpenditures(int? id);
        string GetTransaction(int? id);
        double Total(int id);
        int GenerateInvoiceNo();
        Task<IEnumerable<InvoiceTbl>> DisplayeInvoice();



    }

}
