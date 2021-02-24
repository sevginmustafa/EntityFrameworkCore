using SoftUni.Data;
using System;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var allEmployeesInfo = context.Employees.Where(x => x.Salary > 50000).OrderBy(x => x.FirstName).Select(x => new { x.FirstName, x.Salary });

            StringBuilder sb = new StringBuilder();

            foreach (var employeeInfo in allEmployeesInfo)
            {
                sb.AppendLine($"{employeeInfo.FirstName} - {employeeInfo.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
