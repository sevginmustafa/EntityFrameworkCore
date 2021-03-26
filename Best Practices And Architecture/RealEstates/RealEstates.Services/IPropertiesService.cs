using RealEstates.Services.DTOs;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
        void Add(int size, int yardSize, int floor,
            int totalFloors, string district, int year,
            string propertyType, string buildingType, int price);

        decimal AveragePricePerSquareMeter();

        decimal AveragePricePerSquareMeter(int districtId);

        decimal AverageSize(int districtId);

        IEnumerable<PropertyFullInfoDTO> GetPropertyFullData(int count);

        IEnumerable<PropertyInfoDto> Search(int minPrice, int maxPrice, int minSize, int maxSize);
    }
}
