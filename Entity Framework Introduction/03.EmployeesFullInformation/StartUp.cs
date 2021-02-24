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

            Console.WriteLine(GetEmployeesFullInformation(context));
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var allEmployeesInfo = context.Employees.Select(x => new { x.FirstName, x.LastName, x.MiddleName, x.JobTitle, x.Salary });

            StringBuilder sb = new StringBuilder();

            foreach (var employeeInfo in allEmployeesInfo)
            {
                sb.AppendLine($"{employeeInfo.FirstName} {employeeInfo.LastName} {employeeInfo.MiddleName} {employeeInfo.JobTitle} {employeeInfo.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
