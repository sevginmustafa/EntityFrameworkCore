using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Text;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            var context = new RealEstatesDbContext();

            context.Database.Migrate();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Search property");
                Console.WriteLine("2. Most expensive district");
                Console.WriteLine("3. Average price per square meter");
                Console.WriteLine("4. Add tag");
                Console.WriteLine("5. Add tags to properties");
                Console.WriteLine("0. EXIT");

                int option = int.Parse(Console.ReadLine());
                if (option == 0)
                {
                    Environment.Exit(0);
                }

                if (option > 0 && option <= 6)
                {
                    if (option == 1)
                    {
                        PropertySearch(context);
                    }
                    else if (option == 2)
                    {
                        SearchMostExpensiveDistricts(context);
                    }
                    else if (option == 3)
                    {
                        GetAveragePriceForSquareMeter(context);
                    }
                    else if (option == 4)
                    {
                        AddTag(context);
                    }
                    else if (option == 5)
                    {
                        AddTagsToProperty(context);
                    }
                    else if (option == 6)
                    {

                    }

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private static void AddTagsToProperty(RealEstatesDbContext context)
        {
            Console.WriteLine("Adding tags to properties started!");
            IPropertiesService propertiesService = new PropertiesService(context);
            ITagsService tagsService = new TagsService(context,propertiesService);
            tagsService.BulkAddTagsToProperty();
            Console.WriteLine("Adding tags to properties finished!");
        }

        private static void AddTag(RealEstatesDbContext context)
        {
            Console.WriteLine("Tag name:");
            string tagName = Console.ReadLine();
            Console.WriteLine("Importance (optional):");
            int importance = int.Parse(Console.ReadLine());

            IPropertiesService propertiesService = new PropertiesService(context);
            ITagsService tagsService = new TagsService(context, propertiesService);

            tagsService.Add(tagName, importance);
        }

        private static void GetAveragePriceForSquareMeter(RealEstatesDbContext context)
        {
            IPropertiesService propertiesService = new PropertiesService(context);

            Console.WriteLine($"{propertiesService.AveragePricePerSquareMeter():f2}€/m²");
        }

        private static void PropertySearch(RealEstatesDbContext context)
        {
            Console.WriteLine("min price:");
            int minPrice = int.Parse(Console.ReadLine());
            Console.WriteLine("max price:");
            int maxPrice = int.Parse(Console.ReadLine());
            Console.WriteLine("min size:");
            int minSize = int.Parse(Console.ReadLine());
            Console.WriteLine("max size:");
            int maxSize = int.Parse(Console.ReadLine());

            IPropertiesService propertiesService = new PropertiesService(context);

            var properties = propertiesService.Search(minPrice, maxPrice, minSize, maxSize);

            foreach (var property in properties)
            {
                Console.WriteLine($"{property.DistrictName} => {property.Size} => {property.Price} => {property.PropertyType} => {property.BuildingType}");
            }
        }

        private static void SearchMostExpensiveDistricts(RealEstatesDbContext context)
        {
            Console.WriteLine("Press districts count:");
            int count = int.Parse(Console.ReadLine());

            IDistrictsService districtsService = new DistrictsService(context);

            var districts = districtsService.MostExpensiveDistricts(count);

            foreach (var district in districts)
            {
                Console.WriteLine($"{district.Name} => {district.AveragePricePerSquareMeter:f2}€/m²({district.PropertiesCount})");
            }
        }
    }
}
