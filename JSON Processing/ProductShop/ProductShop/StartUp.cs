using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            //context.Database.EnsureCreated();

            // 01. Import Users 
            //var inputJson = File.ReadAllText(@"../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, inputJson));


            // 02. Import Products 
            //var inputJson = File.ReadAllText(@"../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, inputJson));


            // 03. Import Categories 
            //var inputJson = File.ReadAllText(@"../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, inputJson));


            // 04. Import Categories and Products 
            //var inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputJson));


            // 05. Export Products In Range 
            //var result = GetProductsInRange(context);
            //Console.WriteLine(result);
            //File.WriteAllText(@"../../../Datasets/Results/products-in-range.json", result);


            // 06. Export Sold Products 
            //var result = GetSoldProducts(context);
            //Console.WriteLine(result);
            //File.WriteAllText(@"../../../Datasets/Results/users-sold-products.json", result);


            // 07. Export Categories By Products Count
            //var result = GetCategoriesByProductsCount(context);
            //Console.WriteLine(result);
            //File.WriteAllText(@"../../../Datasets/Results/categories-by-products.json", result);


            // 08. Export Users and Products 
            //var result = GetUsersWithProducts(context);
            //Console.WriteLine(result);
            //File.WriteAllText(@"../../../Datasets/Results/users-and-products.json", result);
        }


        // 01. Import Users 
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var jsonUsers = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(jsonUsers);

            context.SaveChanges();

            return $"Successfully imported {jsonUsers.Count}";
        }


        // 02. Import Products 
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var jsonProducts = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(jsonProducts);

            context.SaveChanges();

            return $"Successfully imported {jsonProducts.Count}";
        }


        // 03. Import Categories 
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var jsonCategories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x => x.Name != null)
                .ToList();

            context.Categories.AddRange(jsonCategories);

            context.SaveChanges();

            return $"Successfully imported {jsonCategories.Count}";
        }


        // 04. Import Categories and Products 
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var jsonCategoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(jsonCategoryProducts);

            context.SaveChanges();

            return $"Successfully imported {jsonCategoryProducts.Count}";
        }


        // 05. Export Products In Range 
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.price)
                .ToList();

            var jsonProducts = JsonConvert.SerializeObject(products, Formatting.Indented);

            return jsonProducts;
        }


        // 06. Export Sold Products 
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(y => y.BuyerId.HasValue))
                .Select(user => new
                {
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    soldProducts = user.ProductsSold
                    .Where(x => x.BuyerId.HasValue)
                    .Select(product => new
                    {
                        name = product.Name,
                        price = product.Price,
                        buyerFirstName = product.Buyer.FirstName,
                        buyerLastName = product.Buyer.LastName
                    })
                })
                .OrderBy(x => x.lastName)
                .ThenBy(x => x.firstName)
                .ToList();

            var jsonUsers = JsonConvert.SerializeObject(users, Formatting.Indented);

            return jsonUsers;
        }


        // 07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = $"{c.CategoryProducts.Average(p => p.Product.Price):f2}",
                    totalRevenue = $"{c.CategoryProducts.Sum(p => p.Product.Price):f2}"
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();

            var jsonCategories = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return jsonCategories;
        }


        // 08. Export Users and Products 
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .AsEnumerable()
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Where(p => p.BuyerId.HasValue).Count(),
                        products = u.ProductsSold.Where(p => p.BuyerId.HasValue).Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    },
                })
                .OrderByDescending(x => x.soldProducts.count)
                .ToList();

            var result = new
            {
                usersCount = context.Users.Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue)).Count(),
                users = users
            };

            var jsonUsers = JsonConvert.SerializeObject(result,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return jsonUsers;
        }
    }
}