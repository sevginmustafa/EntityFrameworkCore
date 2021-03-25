using Microsoft.EntityFrameworkCore;
using RealEstates.Models;

namespace RealEstates.Data
{
    public class RealEstatesDbContext : DbContext
    {
        public RealEstatesDbContext()
        {
        }

        public RealEstatesDbContext(DbContextOptions<RealEstatesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<BuildingType> BuildingTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PropertyTag> PropertyTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS; Database=RealEstates; Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyTag>().HasKey(x => new { x.PropertyId, x.TagId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
