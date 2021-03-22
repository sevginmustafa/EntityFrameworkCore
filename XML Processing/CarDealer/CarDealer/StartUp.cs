using CarDealer.Data;
using CarDealer.DTO.Export;
using CarDealer.DTO.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new CarDealerContext();

            //context.Database.EnsureCreated();


            //09. Import Suppliers
            //var inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, inputXml));


            //10. Import Parts
            //var inputXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, inputXml));


            //11. Import Cars
            //var inputXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, inputXml));


            //12.Import Customers
            //var inputXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, inputXml));


            //13. Import Sales
            //var inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, inputXml));


            //14. Export Cars With Distance
            //var result = GetCarsWithDistance(context);
            //File.WriteAllText("../../../Datasets/Results/cars.xml", result);


            //15. Export Cars From Make BMW
            //var result = GetCarsFromMakeBmw(context);
            //File.WriteAllText("../../../Datasets/Results/bmw-cars.xml", result);


            //16. Export Local Suppliers 
            //var result = GetLocalSuppliers(context);
            //File.WriteAllText("../../../Datasets/Results/local-suppliers.xml", result);


            //17. Export Cars With Their List Of Parts
            //var result = GetCarsWithTheirListOfParts(context);
            //File.WriteAllText("../../../Datasets/Results/cars-and-parts.xml", result);


            //18.Export Total Sales By Customer             
            var result = GetTotalSalesByCustomer(context);
            File.WriteAllText("../../../Datasets/Results/customers-total-sales.xml", result);


            //19. Export Sales With Applied Discount
            //var result = GetSalesWithAppliedDiscount(context);
            //File.WriteAllText("../../../Datasets/Results/sales-discounts.xml", result);
        }


        //09. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            var deserializedSuppliers = (ImportSupplierDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var deserializedSupplier in deserializedSuppliers)
            {
                Supplier supplier = new Supplier
                {
                    Name = deserializedSupplier.Name,
                    IsImporter = deserializedSupplier.IsImporter
                };

                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }


        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var supplierIds = context.Suppliers.Select(x => x.Id).ToList();

            var serializer = new XmlSerializer(typeof(ImportPartDTO[]), new XmlRootAttribute("Parts"));

            var deserializedParts = (ImportPartDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Part> parts = new List<Part>();

            foreach (var deserializedPart in deserializedParts)
            {
                if (supplierIds.Contains(deserializedPart.SupplierId))
                {
                    Part part = new Part
                    {
                        Name = deserializedPart.Name,
                        Price = deserializedPart.Price,
                        Quantity = deserializedPart.Quantity,
                        SupplierId = deserializedPart.SupplierId
                    };

                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);

            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }


        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var partIds = context.Parts.Select(x => x.Id).ToList();

            var serializer = new XmlSerializer(typeof(ImportCarDTO[]), new XmlRootAttribute("Cars"));

            var deserializedCars = (ImportCarDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Car> cars = new List<Car>();

            foreach (var deserializedCar in deserializedCars)
            {
                Car car = new Car
                {
                    Make = deserializedCar.Make,
                    Model = deserializedCar.Model,
                    TravelledDistance = deserializedCar.TravelledDistance
                };

                foreach (var deserializedPartCar in deserializedCar.PartCars.Select(x => x.PartId).Distinct().Where(x => partIds.Contains(x)))
                {
                    PartCar partCar = new PartCar
                    {
                        PartId = deserializedPartCar
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);

            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }


        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));

            var deserializedCustomers = (ImportCustomerDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Customer> customers = new List<Customer>();

            foreach (var deserializedCustomer in deserializedCustomers)
            {
                Customer customer = new Customer
                {
                    Name = deserializedCustomer.Name,
                    BirthDate = deserializedCustomer.BirthDate,
                    IsYoungDriver = deserializedCustomer.IsYoungDriver
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);

            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }


        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var carIds = context.Cars.Select(x => x.Id).ToList();

            var serializer = new XmlSerializer(typeof(ImportSaleDTO[]), new XmlRootAttribute("Sales"));

            var deserializedSales = (ImportSaleDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Sale> sales = new List<Sale>();

            foreach (var deserializedSale in deserializedSales)
            {
                if (carIds.Contains(deserializedSale.CarId))
                {
                    Sale sale = new Sale
                    {
                        CarId = deserializedSale.CarId,
                        CustomerId = deserializedSale.CustomerId,
                        Discount = deserializedSale.Discount
                    };

                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);

            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }


        //14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var cars = context.Cars
                .Where(x => x.TravelledDistance > 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .Select(x => new ExportCarsWithDistanceDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsWithDistanceDTO[]), new XmlRootAttribute("cars"));

            var writer = new StringWriter();

            serializer.Serialize(writer, cars, namespaces);

            return writer.ToString();
        }


        //15. Export Cars From Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var cars = context.Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new ExportBMWCarsDTO
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportBMWCarsDTO[]), new XmlRootAttribute("cars"));

            var writer = new StringWriter();

            serializer.Serialize(writer, cars, namespaces);

            return writer.ToString();
        }


        //16. Export Local Suppliers 
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new ExportLocalSuppliersDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportLocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));

            var writer = new StringWriter();

            serializer.Serialize(writer, suppliers, namespaces);

            return writer.ToString();
        }


        //17. Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var cars = context.Cars
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .Select(x => new ExportCarsWithPartsDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    PartCars = x.PartCars.Select(p => new ExportCarPartsDTO
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(x => x.Price)
                    .ToArray()
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCarsWithPartsDTO[]), new XmlRootAttribute("cars"));

            var writer = new StringWriter();

            serializer.Serialize(writer, cars, namespaces);

            return writer.ToString();
        }


        //18. Export Total Sales By Customer 
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var customers = context.Customers
                .Where(x => x.Sales.Count > 0)
                .Select(x => new ExportCustomerTotalSalesDTO
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.SelectMany(s => s.Car.PartCars.Select(x => x.Part.Price)).Sum()//works on all versions of "Microsoft.EntityFrameworkCore"
                    //x.Sales.Sum(s=>s.Car.PartCars.Sum(p=>p.Part.Price))--doesn't work on -v 3.1.3 but somehow works on older versions like 2.1.1 for example
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportCustomerTotalSalesDTO[]), new XmlRootAttribute("customers"));

            var writer = new StringWriter();

            serializer.Serialize(writer, customers, namespaces);

            return writer.ToString();
        }


        //19. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            var sales = context.Sales
                .Select(s => new ExportSalesWithAppliedDiscountDTO
                {
                    Car = new ExportCarSalesDiscountDTO
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    Discount = s.Discount,
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(x => x.Part.Price),
                    PriceWithDiscount = (double)(s.Car.PartCars.Sum(x => x.Part.Price) * (1 - s.Discount / 100))
                })
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportSalesWithAppliedDiscountDTO[]), new XmlRootAttribute("sales"));

            var writer = new StringWriter();

            serializer.Serialize(writer, sales, namespaces);

            return writer.ToString();
        }
    }
}