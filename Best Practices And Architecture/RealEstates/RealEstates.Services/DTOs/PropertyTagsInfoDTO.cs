using System.Xml.Serialization;

namespace RealEstates.Services.DTOs
{
    [XmlType("Tag")]
    public class PropertyTagsInfoDTO
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
    }
}
