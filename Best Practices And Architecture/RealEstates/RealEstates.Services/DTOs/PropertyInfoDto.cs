﻿using System.Xml.Serialization;

namespace RealEstates.Services.DTOs
{
    [XmlType]
    public class PropertyInfoDto
    {
        [XmlElement("DistrictName")]
        public string DistrictName { get; set; }

        [XmlElement("Size")]
        public int Size { get; set; }

        [XmlElement("Price")]
        public int Price { get; set; }

        [XmlElement("PropertyType")]
        public string PropertyTypeName { get; set; }

        [XmlElement("BuildingType")]
        public string BuildingTypeName { get; set; }
    }
}
