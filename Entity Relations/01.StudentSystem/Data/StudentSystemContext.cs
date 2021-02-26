using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions<StudentSystemContext> options) :
            base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS; Database = StudentSystem; Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.PhoneNumber)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .IsUnicode(false);

                entity.Property(x => x.Birthday).IsRequired(false);
            });

            modelBuilder.Entity<Course>()
                    .Property(x => x.Name)
                    .HasMaxLength(80)
                    .IsRequired();
           
            modelBuilder.Entity<Resource>(entitiy =>
            {
                entitiy.Property(x => x.Name)
                    .HasMaxLength(50)
                    .IsRequired();

                entitiy.Property(x => x.Url).IsUnicode(false);
            });

            modelBuilder.Entity<Homework>()
                    .Property(x => x.Content)
                    .IsUnicode(false)
                    .IsRequired();

            modelBuilder.Entity<StudentCourse>().HasKey(x => new { x.StudentId, x.CourseId });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Homework> HomeworkSubmissions { get; set; }
        public virtual DbSet<StudentCourse> StudentCourses { get; set; }
    }
}
