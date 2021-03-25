using RealEstates.Services.DTOs;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IDistrictsService
    {
        IEnumerable<DistrictInfoDTO> MostExpensiveDistricts(int count);
    }
}
