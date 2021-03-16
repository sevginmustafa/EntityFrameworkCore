using Newtonsoft.Json;

namespace CarDealer.DTO
{
    public  class TotalCustomerSalesOutputModel
    {
        [JsonProperty("fullName")]
        public string Name { get; set; }

        [JsonProperty("boughtCars")]
        public int SalesCount { get; set; }

        [JsonProperty("spentMoney")]
        public decimal SpentMoney { get; set; }
    }
}
