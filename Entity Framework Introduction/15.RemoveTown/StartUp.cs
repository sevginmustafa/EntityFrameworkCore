using SoftUni.Data;
using System;
using System.Linq;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            Console.WriteLine(RemoveTown(context));
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var allEmployees = context.Employees.ToList();

            allEmployees.ForEach(x => x.AddressId = null);

            var town = context.Towns.First(x => x.Name == "Seattle");

            var addresses = context.Addresses.Where(x => x.Town == town).ToList();

            context.Addresses.RemoveRange(addresses);

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{addresses.Count} addresses in Seattle were deleted";
        }
    }
}
