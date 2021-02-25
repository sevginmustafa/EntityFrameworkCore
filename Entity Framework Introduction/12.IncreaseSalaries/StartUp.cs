using SoftUni.Data;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(IncreaseSalaries(context));
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employeesInfo = context.Employees
                .Where(x =>
                x.Department.Name == "Engineering" ||
                x.Department.Name == "Tool Design" ||
                x.Department.Name == "Marketing" ||
                x.Department.Name == "Information Services")
                .ToList();

            employeesInfo.ForEach(x => x.Salary *= 1.12m);

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var employeeInfo in employeesInfo.OrderBy(x=>x.FirstName).ThenBy(x=>x.LastName))
            {
                sb.AppendLine($"{employeeInfo.FirstName} {employeeInfo.LastName} (${employeeInfo.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
