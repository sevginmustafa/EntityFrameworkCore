using RealEstates.Data;
using RealEstates.Services.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace RealEstates.Services
{
    public class DistrictsService : IDistrictsService
    {
        private readonly RealEstatesDbContext context;

        public DistrictsService(RealEstatesDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DistrictInfoDTO> MostExpensiveDistricts(int count)
        {
            var districts = context.Districts
                 .Select(x => new DistrictInfoDTO
                 {
                     Name = x.Name,
                     AveragePricePerSquareMeter = x.Properties.Average(p => p.Price.Value / (double)p.Size),
                     PropertiesCount = x.Properties.Count
                 })
                 .OrderByDescending(x => x.AveragePricePerSquareMeter)
                 .Take(count)
                 .ToList();

            return districts;
        }
    }
}
