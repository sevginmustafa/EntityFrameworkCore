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

            Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departmentsInfo = context.Departments
                .Where(x => x.Employees.Count() > 5)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(x => new
                    {
                        EmployeeFirstName = x.FirstName,
                        EmployeeLastName = x.LastName,
                        EmployeeJobTitle = x.JobTitle,
                    })
                    .OrderBy(x => x.EmployeeFirstName)
                    .ThenBy(x => x.EmployeeLastName)
                    .ToList()
                })
                .ToList()
                .OrderBy(x => x.Employees.Count())
                .ThenBy(x => x.DepartmentName);

            StringBuilder sb = new StringBuilder();

            foreach (var departmentInfo in departmentsInfo)
            {
                sb.AppendLine($"{departmentInfo.DepartmentName} - {departmentInfo.ManagerFirstName} {departmentInfo.ManagerLastName}");

                foreach (var employee in departmentInfo.Employees)
                {
                    sb.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - {employee.EmployeeJobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
