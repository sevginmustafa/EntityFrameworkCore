using System.Xml.Serialization;

namespace CarDealer.DTO.Import
{
    [XmlType("partId")]
    public  class ImportPartCarDTO
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
