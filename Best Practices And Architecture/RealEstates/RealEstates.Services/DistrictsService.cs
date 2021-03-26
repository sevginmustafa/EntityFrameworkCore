using RealEstates.Data;
using RealEstates.Services.DTOs;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;

namespace RealEstates.Services
{
    public class DistrictsService : BaseService, IDistrictsService
    {
        private readonly RealEstatesDbContext context;

        public DistrictsService(RealEstatesDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<DistrictInfoDTO> MostExpensiveDistricts(int count)
        {
            var districts = context.Districts
                 .ProjectTo<DistrictInfoDTO>(Mapper.ConfigurationProvider)
                 .OrderByDescending(x => x.AveragePricePerSquareMeter)
                 .Take(count)
                 .ToArray();

            return districts;
        }
    }
}
