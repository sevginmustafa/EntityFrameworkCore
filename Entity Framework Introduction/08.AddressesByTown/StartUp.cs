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

            Console.WriteLine(GetAddressesByTown(context));
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var allAddressesInfo = context.Addresses
                .OrderByDescending(x => x.Employees.Count())
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .Select(x => new
                {
                    x.AddressText,
                    TownName = x.Town.Name,
                    EmployeeCount = x.Employees.Count()
                });

            StringBuilder sb = new StringBuilder();

            foreach (var addressInfo in allAddressesInfo)
            {
                sb.AppendLine($"{addressInfo.AddressText}, {addressInfo.TownName} - {addressInfo.EmployeeCount} employees");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
