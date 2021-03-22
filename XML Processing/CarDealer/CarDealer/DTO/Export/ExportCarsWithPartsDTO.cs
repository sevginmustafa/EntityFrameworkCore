using System.Xml.Serialization;

namespace CarDealer.DTO.Export
{
    [XmlType("car")]
    public class ExportCarsWithPartsDTO
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public ExportCarPartsDTO[] PartCars { get; set; }
    }
}
