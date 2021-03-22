using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public  class UsersRootOutputModel
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UsersAndProductsOutputModel[] UsersAndProducts { get; set; }
    }
}
