using Microsoft.EntityFrameworkCore;
using Orari.Models;

namespace Orari.DataDbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Students> Students { get; set; }
        public DbSet<Profesors> Profesors { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<StudyPrograms> StudyPrograms { get; set; }
        public DbSet<StudyProgramCourse> StudyProgramCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship between Exams and Schedules
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Schedule)
                .WithOne(s => s.Exam)
                .HasForeignKey<Schedules>(s => s.EId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
