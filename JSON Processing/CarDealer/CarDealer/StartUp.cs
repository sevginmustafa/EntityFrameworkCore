using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CarDealer
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //context.Database.EnsureCreated();

            //09. Import Suppliers
            //var inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, inputJson));


            //10.Import Parts
            //var inputJson = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, inputJson));


            //11.Import Cars
            //var inputJson = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, inputJson));


            //12.Import Customers
            //var inputJson = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, inputJson));


            //13.Import Sales
            //var inputJson = File.ReadAllText("../../../Datasets/sales.json");
            //Console.WriteLine(ImportSales(context, inputJson));


            //14. Export Ordered Customers
            //var result = GetOrderedCustomers(context);
            //File.WriteAllText("../../../Datasets/Results/ordered-customers.json", result);


            //15.Export Cars From Make Toyota
            //var result = GetCarsFromMakeToyota(context);
            //File.WriteAllText("../../../Datasets/Results/toyota-cars.json", result);


            //16. Export Local Suppliers
            //var result = GetLocalSuppliers(context);
            //File.WriteAllText("../../../Datasets/Results/local-suppliers.json", result);


            //17.Export Cars With Their List Of Parts
            //var result = GetCarsWithTheirListOfParts(context);
            //File.WriteAllText("../../../Datasets/Results/cars-and-parts.json", result);


            //18. Export Total Sales By Customer
            //var result = GetTotalSalesByCustomer(context);
            //File.WriteAllText("../../../Datasets/Results/customers-total-sales.json", result);


            //19.Export Sales With Applied Discount
            //var result = GetSalesWithAppliedDiscount(context);
            //File.WriteAllText("../../../Datasets/Results/sales-discounts.json", result);
        }


        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var dtoSuppliers = JsonConvert.DeserializeObject<IEnumerable<SupplierInputModel>>(inputJson);

            var suppliers = mapper.Map<IEnumerable<Supplier>>(dtoSuppliers);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";
        }


        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var suppliersIds = context.Suppliers
                .Select(x => x.Id)
                .ToList();

            var dtoParts = JsonConvert.DeserializeObject<IEnumerable<PartInputModel>>(inputJson)
                .Where(x => suppliersIds.Contains(x.SupplierId));

            var parts = mapper.Map<IEnumerable<Part>>(dtoParts);

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count()}.";
        }


        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var dtoCars = JsonConvert.DeserializeObject<IEnumerable<CarInputModel>>(inputJson);

            var cars = new List<Car>();

            //with mapper
            foreach (var dtoCar in dtoCars)
            {
                var car = mapper.Map<Car>(dtoCar);

                foreach (var partId in dtoCar.PartsId)
                {
                    car.PartCars.Add(new PartCar { PartId = partId });
                }

                cars.Add(car);
            }

            //without mapper
            //foreach (var dtoCar in dtoCars)
            //{
            //    var car = new Car
            //    {
            //        Model = dtoCar.Model,
            //        Make = dtoCar.Make,
            //        TravelledDistance = dtoCar.TravelledDistance,
            //    };

            //    foreach (var partId in dtoCar.PartsId)
            //    {
            //        car.PartCars.Add(new PartCar() { PartId = partId });
            //    }

            //    cars.Add(car);
            //}

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count()}.";
        }


        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var dtoCustomers = JsonConvert.DeserializeObject<IEnumerable<CustomerInputModel>>(inputJson);

            var customers = mapper.Map<IEnumerable<Customer>>(dtoCustomers);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count()}.";
        }


        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeMapper();

            var dtoSales = JsonConvert.DeserializeObject<IEnumerable<SaleInputModel>>(inputJson);

            var sales = mapper.Map<IEnumerable<Sale>>(dtoSales);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count()}.";
        }


        //14. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            InitializeMapper();

            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .ProjectTo<OrderedCustomersOutputModel>(mapper.ConfigurationProvider)
                .ToList();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }


        //15. Export Cars From Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            InitializeMapper();

            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ProjectTo<ToyotaCarsOutputModel>(mapper.ConfigurationProvider)
                .ToList();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }


        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            InitializeMapper();

            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .ProjectTo<LocalSuppliersOutputModel>(mapper.ConfigurationProvider)
                .ToList();

            var result = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return result;
        }


        //17. Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars
                    .Select(p => new
                    {
                        p.Part.Name,
                        Price = p.Part.Price.ToString("f2")
                    })
                })
                .ToList();

            var result = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return result;
        }


        //18. Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            InitializeMapper();

            var customers = context.Customers
                .Where(x => x.Sales.Count > 0)
                .ProjectTo<TotalCustomerSalesOutputModel>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.SpentMoney)
                .ThenByDescending(x => x.SalesCount)
                .ToList();

            var result = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return result;
        }


        //19. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(x => new
                {
                    car = new
                    {
                        x.Car.Make,
                        x.Car.Model,
                        x.Car.TravelledDistance
                    },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("f2"),
                    price = x.Car.PartCars.Sum(p => p.Part.Price).ToString("f2"),
                    priceWithDiscount = (x.Car.PartCars.Sum(p => p.Part.Price) * (1 - (x.Discount / 100))).ToString("f2")
                });

            var result = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return result;
        }


        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
    }
}