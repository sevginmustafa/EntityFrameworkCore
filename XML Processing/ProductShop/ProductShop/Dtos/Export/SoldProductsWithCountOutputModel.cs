using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("SoldProducts")]
    public class SoldProductsWithCountOutputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public SoldProductsOutputModel[] SoldProducts { get; set; }
    }
}
