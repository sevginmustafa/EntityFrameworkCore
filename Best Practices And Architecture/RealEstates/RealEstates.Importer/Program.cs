using RealEstates.Data;
using RealEstates.Services;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RealEstates.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportJsonFile("imot.bg-houses-Sofia-raw-data-2021-03-18.json");
            System.Console.WriteLine();
            ImportJsonFile("imot.bg-raw-data-2021-03-18.json");
        }

        public static void ImportJsonFile(string fileName)
        {
            var context = new RealEstatesDbContext();

            IPropertiesService propertiesService = new PropertiesService(context);

            var jsonConverter = JsonSerializer.Deserialize<IEnumerable<PropertyAsJson>>(
             File.ReadAllText(fileName));

            foreach (var item in jsonConverter)
            {
                propertiesService.Add(item.Size, item.YardSize, item.Floor, 
                    item.TotalFloors, item.District, item.Year, 
                    item.Type, item.BuildingType, item.Price);
                System.Console.Write("-");
            }
        }
    }
}
