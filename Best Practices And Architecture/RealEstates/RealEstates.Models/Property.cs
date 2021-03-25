using System.Collections.Generic;

namespace RealEstates.Models
{
    public class Property
    {
        public Property()
        {
            PropertyTags = new HashSet<PropertyTag>();
        }

        public int Id { get; set; }

        public int Size { get; set; }

        public int? YardSize { get; set; }

        public int? Floor { get; set; }

        public int? TotalFloors { get; set; }

        public int? Year { get; set; }

        public int? Price { get; set; }

        public int DistrictId { get; set; }

        public District District { get; set; }

        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public int BuildingTypeId { get; set; }

        public virtual BuildingType BuildingType { get; set; }


        public virtual ICollection<PropertyTag> PropertyTags { get; set; }
    }
}
