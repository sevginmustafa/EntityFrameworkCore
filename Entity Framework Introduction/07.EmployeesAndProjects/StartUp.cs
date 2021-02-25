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

            Console.WriteLine(GetEmployeesInPeriod(context));
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var allEmployeesInfo = context.Employees.
                Where(x => x.EmployeesProjects.
                    Any(x => x.Project.StartDate.Year >= 2001 && x.Project.StartDate.Year <= 2003)).
                Take(10).
                Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.
                    Select(x => new
                    {
                        ProjectName = x.Project.Name,
                        StartDate = x.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = x.Project.EndDate.HasValue ? x.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                    }).ToList()
                }).
                ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employeeInfo in allEmployeesInfo)
            {
                sb.AppendLine($"{employeeInfo.FirstName} {employeeInfo.LastName} - Manager: {employeeInfo.ManagerFirstName} {employeeInfo.ManagerLastName}");

                foreach (var employeeProjects in employeeInfo.Projects)
                {
                    sb.AppendLine($"--{employeeProjects.ProjectName} - {employeeProjects.StartDate} - {employeeProjects.EndDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
