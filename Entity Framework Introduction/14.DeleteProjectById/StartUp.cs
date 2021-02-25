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

            Console.WriteLine(DeleteProjectById(context));
        }

        public static string DeleteProjectById(SoftUniContext context)
        {

            var employeeProject = context.EmployeesProjects.Where(x => x.ProjectId == 2).ToList();

            context.EmployeesProjects.RemoveRange(employeeProject);

            var project = context.Projects.Find(2);

            context.Projects.Remove(project);

            context.SaveChanges();

            StringBuilder sb = new StringBuilder();

            foreach (var p in context.Projects.Take(10))
            {
                sb.AppendLine(p.Name);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
