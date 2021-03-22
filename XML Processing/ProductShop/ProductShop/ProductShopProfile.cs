using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Input Models
            this.CreateMap<UserInputModel, User>();

            this.CreateMap<ProductInputModel, Product>();

            this.CreateMap<CategoryInputModel, Category>();

            this.CreateMap<CategoryProductsInputModel, CategoryProduct>();


            //Output Models
            this.CreateMap<Product, ProductsInRangeOutputModel>()
                .ForMember(x => x.BuyerFullName, y => y.MapFrom(s => s.Buyer.FirstName + " " + s.Buyer.LastName));

            this.CreateMap<User, UserSoldProductOutputModel>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(s => s.ProductsSold));

            this.CreateMap<Category, CategoriesByProductsCountOutputModel>()
                .ForMember(x => x.ProductsCount, y => y.MapFrom(s => s.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice, y => y.MapFrom(s => s.CategoryProducts.Average(x => x.Product.Price)))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(s => s.CategoryProducts.Sum(x => x.Product.Price)));
        }
    }
}
