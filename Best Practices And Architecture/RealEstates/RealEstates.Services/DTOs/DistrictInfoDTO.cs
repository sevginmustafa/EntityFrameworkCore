using System.Xml.Serialization;

namespace RealEstates.Services.DTOs
{
    [XmlType("District")]
    public class DistrictInfoDTO
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("AveragePricePerSquareMeter")]
        public double AveragePricePerSquareMeter { get; set; }

        [XmlElement("PropertiesCount")]
        public int PropertiesCount { get; set; }
    }
}
