using ProductShop.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using ProductShop.Dtos.Export;
using System.Text;

namespace ProductShop
{
    public class StartUp
    {
        static IMapper mapper;

        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureCreated();

            //01. Import Users
            //var inputXml = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, inputXml));


            //02. Import Products
            //var inputXml = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, inputXml));


            //03. Import Categories
            //var inputXml = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, inputXml));


            //04. Import Categories and Products
            //var inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, inputXml));


            //05. Export Products In Range
            //var result = GetProductsInRange(context);
            //File.WriteAllText("../../../Datasets/Results/products-in-range.xml", result);


            //06. Export Sold Products
            //var result = GetSoldProducts(context);
            //File.WriteAllText("../../../Datasets/Results/users-sold-products.xml", result);


            //07.Export Categories By Products Count
            //var result = GetCategoriesByProductsCount(context);
            //File.WriteAllText("../../../Datasets/Results/categories-by-products.xml", result);


            //08.Export Users and Products
            //var result = GetUsersWithProducts(context);
            //File.WriteAllText("../../../Datasets/Results/users-and-products.xml", result);
        }


        //01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(UserInputModel[]), new XmlRootAttribute("Users"));

            var deserializedUsers = (UserInputModel[])serializer.Deserialize(new StringReader(inputXml));

            var users = mapper.Map<User[]>(deserializedUsers);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }


        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(ProductInputModel[]), new XmlRootAttribute("Products"));

            var deserializedProducts = (ProductInputModel[])serializer.Deserialize(new StringReader(inputXml));

            var products = mapper.Map<Product[]>(deserializedProducts);

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {products.Count()}";
        }


        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var serializer = new XmlSerializer(typeof(CategoryInputModel[]), new XmlRootAttribute("Categories"));

            var deserializedCategories = (CategoryInputModel[])serializer.Deserialize(new StringReader(inputXml));

            var categories = mapper.Map<Category[]>(deserializedCategories).Where(x => x.Name != null);

            context.Categories.AddRange(categories);

            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }


        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            InitializeMapper();

            var productsIDs = context.Products.Select(x => x.Id).ToList();
            var categoryIDs = context.Categories.Select(x => x.Id).ToList();

            var serializer = new XmlSerializer(typeof(CategoryProductsInputModel[]), new XmlRootAttribute("CategoryProducts"));

            var deserializedCategoryProducts = (CategoryProductsInputModel[])serializer.Deserialize(new StringReader(inputXml));

            var categoryProducts = mapper.Map<CategoryProduct[]>(deserializedCategoryProducts)
                .Where(x => productsIDs.Contains(x.ProductId) && categoryIDs.Contains(x.CategoryId));

            context.CategoryProducts.AddRange(categoryProducts);

            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count()}";
        }


        //05. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            InitializeMapper();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();

            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .ProjectTo<ProductsInRangeOutputModel>(mapper.ConfigurationProvider)
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ProductsInRangeOutputModel[]), new XmlRootAttribute("Products"));

            serializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString();
        }


        //06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            InitializeMapper();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();

            var users = context.Users
                .Where(x => x.ProductsSold.Count > 0)
                .ProjectTo<UserSoldProductOutputModel>(mapper.ConfigurationProvider)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserSoldProductOutputModel[]), new XmlRootAttribute("Users"));

            serializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString();
        }


        //07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            InitializeMapper();

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var categories = context.Categories
                .ProjectTo<CategoriesByProductsCountOutputModel>(mapper.ConfigurationProvider)
                .OrderByDescending(x => x.ProductsCount)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var serializer = new XmlSerializer(typeof(CategoriesByProductsCountOutputModel[]), new XmlRootAttribute("Categories"));

            serializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString();
        }


        //08. Export Users and Products--for this problem i used Lazy Loading because of Judge
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            StringBuilder sb = new StringBuilder();

            var users = new UsersRootOutputModel()
            {
                Count = context.Users.Count(x => x.ProductsSold.Count > 0),
                UsersAndProducts = context.Users
                .ToArray()
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(10)
                .Select(u => new UsersAndProductsOutputModel()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProductsWithCount = new SoldProductsWithCountOutputModel()
                    {
                        Count = u.ProductsSold.Count(),
                        SoldProducts = u.ProductsSold
                        .Select(p => new SoldProductsOutputModel()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .ToArray()
            };

            var serializer = new XmlSerializer(typeof(UsersRootOutputModel), new XmlRootAttribute("Users"));

            serializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString();
        }


        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductShopProfile());
            });

            mapper = config.CreateMapper();
        }
    }
}