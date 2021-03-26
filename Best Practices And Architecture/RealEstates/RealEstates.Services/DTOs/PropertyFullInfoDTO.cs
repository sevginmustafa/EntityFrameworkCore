using System.Xml.Serialization;

namespace RealEstates.Services.DTOs
{
    [XmlType("Property")]
    public class PropertyFullInfoDTO
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("DistrictName")]
        public string DistrictName { get; set; }

        [XmlElement("Size")]
        public int Size { get; set; }

        [XmlElement("Year")]
        public int? Year { get; set; }

        [XmlElement("Price")]
        public int? Price { get; set; }

        [XmlElement("PropertyType")]
        public string PropertyTypeName { get; set; }

        [XmlElement("BuildingType")]
        public string BuildingTypeName { get; set; }

        [XmlArray("Tags")]
        public virtual PropertyTagsInfoDTO[] PropertyTags { get; set; }
    }
}
