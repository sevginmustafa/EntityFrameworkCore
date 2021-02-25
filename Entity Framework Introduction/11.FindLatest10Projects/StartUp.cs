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

            Console.WriteLine(GetLatestProjects(context));
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projectsInfo = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .Select(x => new
                {
                    x.Name,
                    x.Description,
                    StartDate = x.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .ToList()
                .OrderBy(x=>x.Name);

            StringBuilder sb = new StringBuilder();

            foreach (var projectInfo in projectsInfo)
            {
                sb.AppendLine(projectInfo.Name);
                sb.AppendLine(projectInfo.Description);
                sb.AppendLine(projectInfo.StartDate);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
