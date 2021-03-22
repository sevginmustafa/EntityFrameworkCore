using CarDealer.Data;
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
            var inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            Console.WriteLine(ImportSales(context, inputXml));
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
    }
}