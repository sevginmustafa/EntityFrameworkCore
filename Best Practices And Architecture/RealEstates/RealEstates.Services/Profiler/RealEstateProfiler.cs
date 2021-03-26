using AutoMapper;
using RealEstates.Models;
using RealEstates.Services.DTOs;
using System.Linq;

namespace RealEstates.Services.Profiler
{
    public class RealEstateProfiler : Profile
    {
        public RealEstateProfiler()
        {
            this.CreateMap<District, DistrictInfoDTO>()
                .ForMember(x => x.AveragePricePerSquareMeter, y => y.MapFrom(s => s.Properties
                     .Where(x => x.Price.HasValue)
                     .Average(p => p.Price.Value / (double)p.Size)));

            this.CreateMap<Property, PropertyInfoDto>();

            this.CreateMap<PropertyTag, PropertyTagsInfoDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Tag.Name));

            this.CreateMap<Property, PropertyFullInfoDTO>();
        }
    }
}
