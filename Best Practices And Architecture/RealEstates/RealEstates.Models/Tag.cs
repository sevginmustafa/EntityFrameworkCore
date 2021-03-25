using System.Collections.Generic;

namespace RealEstates.Models
{
    public class Tag
    {
        public Tag()
        {
            PropertyTags = new HashSet<PropertyTag>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? Importance { get; set; }


        public virtual ICollection<PropertyTag> PropertyTags { get; set; }
    }
}
