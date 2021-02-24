using SoftUni.Data;
using SoftUni.Models;
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

            Console.WriteLine(AddNewAddressToEmployee(context));
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address newAddress = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            context.Addresses.Add(newAddress);

            var employeeNakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            employeeNakov.Address = newAddress;

            context.SaveChanges();

            var allEmployeesInfo = context.Employees.OrderByDescending(x => x.AddressId).Take(10).Select(x => x.Address.AddressText);

            StringBuilder sb = new StringBuilder();

            foreach (var employeeInfo in allEmployeesInfo)
            {
                sb.AppendLine(employeeInfo);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
