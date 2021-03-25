using RealEstates.Data;
using RealEstates.Models;
using System;
using System.Linq;

namespace RealEstates.Services
{
    public class TagsService : BaseService, ITagsService
    {
        private readonly RealEstatesDbContext context;
        private readonly IPropertiesService propertiesService;

        public TagsService(RealEstatesDbContext context, IPropertiesService propertiesService)
        {
            this.context = context;
            this.propertiesService = propertiesService;
        }

        public void Add(string name, int? importance = null)
        {
            Tag tag = new Tag
            {
                Name = name,
                Importance = importance <= 0 || importance > 10 ? null : importance
            };

            context.Tags.Add(tag);
            context.SaveChanges();
        }

        public void BulkAddTagsToProperty()
        {
            var properties = context.Properties.ToList();

            foreach (var property in properties)
            {
                var averagePriceForDistrict = propertiesService.AveragePricePerSquareMeter(property.DistrictId);

                if (property.Price.HasValue && property.Price >= averagePriceForDistrict)
                {
                    var tag = GetTag("скъп-имот");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }
                else
                {
                    var tag = GetTag("евтин-имот");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }

                var date = DateTime.Now.AddYears(-15);

                if (property.Year.HasValue && property.Year >= date.Year)
                {
                    var tag = GetTag("нов-имот");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }
                else
                {
                    var tag = GetTag("стар-имот");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }

                var averageSize = propertiesService.AverageSize(property.DistrictId);

                if (property.Size >= averageSize)
                {
                    var tag = GetTag("голям-имот");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }
                else
                {
                    var tag = GetTag("малък-имот");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }

                if (property.Floor.HasValue && property.Floor == 1)
                {
                    var tag = GetTag("първи-етаж");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }
                else if (property.Floor.HasValue && property.TotalFloors.HasValue
                    && property.Floor.Value == property.TotalFloors.Value)
                {
                    var tag = GetTag("последен-етаж");
                    property.PropertyTags.Add(new PropertyTag { Tag = tag });
                }
            }

            context.SaveChanges();
        }

        private Tag GetTag(string tagName)
        => context.Tags.FirstOrDefault(x => x.Name == tagName);
    }
}
