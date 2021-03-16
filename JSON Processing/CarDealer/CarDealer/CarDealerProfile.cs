using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //input models
            this.CreateMap<SupplierInputModel, Supplier>();

            this.CreateMap<PartInputModel, Part>();

            this.CreateMap<CarInputModel, Car>();

            this.CreateMap<CustomerInputModel, Customer>();

            this.CreateMap<SaleInputModel, Sale>();


            //output models
            this.CreateMap<Customer, OrderedCustomersOutputModel>()
                .ForMember(x => x.BirthDate, y => y.MapFrom(s => s.BirthDate.ToString("dd/MM/yyyy")));

            this.CreateMap<Car, ToyotaCarsOutputModel>();

            this.CreateMap<Supplier, LocalSuppliersOutputModel>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(s => s.Parts.Count));

            this.CreateMap<Customer, TotalCustomerSalesOutputModel>()
                .ForMember(x => x.SpentMoney, y => y.MapFrom(s => s.Sales.Sum(c => c.Car.PartCars.Sum(p => p.Part.Price))));
        }
    }
}
