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

            Console.WriteLine(GetEmployee147(context));
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employeeInfo = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,
                    Projects = x.EmployeesProjects.Select(x => x.Project.Name).OrderBy(x => x).ToList()
                })
                .ToList(); ;

            StringBuilder sb = new StringBuilder();

            foreach (var info in employeeInfo)
            {
                sb.AppendLine($"{info.FirstName} {info.LastName} - {info.JobTitle}");

                foreach (var projects in info.Projects)
                {
                    sb.AppendLine(projects);
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
