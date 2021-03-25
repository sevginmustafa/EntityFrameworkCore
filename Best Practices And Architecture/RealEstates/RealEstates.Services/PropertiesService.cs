using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly RealEstatesDbContext context;

        public PropertiesService(RealEstatesDbContext context)
        {
            this.context = context;
        }

        public void Add(int size, int yardSize, int floor,
            int totalFloors, string district, int year,
            string propertyType, string buildingType, int price)
        {
            Property property = new Property
            {
                Size = size,
                YardSize = yardSize <= 0 ? null : yardSize,
                Floor = floor <= 0 ? null : floor,
                TotalFloors = totalFloors <= 0 ? null : totalFloors,
                Year = year <= 0 ? null : year,
                Price = price <= 0 ? null : price
            };

            District dbDistrict = context.Districts.FirstOrDefault(x => x.Name == district);
            if (dbDistrict == null)
            {
                dbDistrict = new District { Name = district };
            }
            property.District = dbDistrict;

            PropertyType dbPropertyType = context.PropertyTypes.FirstOrDefault(x => x.Name == propertyType);
            if (dbPropertyType == null)
            {
                dbPropertyType = new PropertyType { Name = propertyType };
            }
            property.PropertyType = dbPropertyType;

            BuildingType dbBuildingType = context.BuildingTypes.FirstOrDefault(x => x.Name == buildingType);
            if (dbBuildingType == null)
            {
                dbBuildingType = new BuildingType { Name = buildingType };
            }
            property.BuildingType = dbBuildingType;

            context.Properties.Add(property);
            context.SaveChanges();
        }

        public decimal AveragePricePerSquareMeter()
       => context.Properties
            .Where(x => x.Price.HasValue)
            .Average(x => x.Price.Value / (decimal)x.Size);

        public decimal AveragePricePerSquareMeter(int districtId)
        => context.Properties
            .Where(x => x.Price.HasValue && x.DistrictId == districtId)
            .Average(x => x.Price.Value / (decimal)x.Size);

        public decimal AverageSize(int districtId)
        => context.Properties
            .Where(x => x.Price.HasValue && x.DistrictId == districtId)
            .Average(x => (decimal)x.Size);

        public IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize)
        {
            var properties = context.Properties
                .Where(x => x.Price >= minPrice && x.Price <= maxPrice && x.Size >= minSize && x.Size <= maxSize)
                .Select(x => new PropertyInfoDto
                {
                    DistrictName = x.District.Name,
                    Size = x.Size,
                    Price = x.Price ?? 0,
                    PropertyType = x.PropertyType.Name,
                    BuildingType = x.BuildingType.Name
                })
                .ToList();

            return properties;
        }
    }
}
