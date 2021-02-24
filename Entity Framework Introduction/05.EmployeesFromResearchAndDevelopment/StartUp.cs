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

            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var allEmployeesInfo = context.Employees.
                Where(x => x.Department.Name == "Research and Development").
                OrderBy(x => x.Salary).
                ThenByDescending(x => x.FirstName).
                Select(x => new { x.FirstName, x.LastName, x.Department.Name, x.Salary });

            StringBuilder sb = new StringBuilder();

            foreach (var employeeInfo in allEmployeesInfo)
            {
                sb.AppendLine($"{employeeInfo.FirstName} {employeeInfo.LastName} from {employeeInfo.Name} - ${employeeInfo.Salary:f2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
