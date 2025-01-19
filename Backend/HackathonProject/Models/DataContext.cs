using Microsoft.EntityFrameworkCore;

namespace HackathonProject.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Student) // Each course belongs to one student
                .WithMany(s => s.Courses) // Each student can have many courses
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete when a student is removed
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
    }
}
