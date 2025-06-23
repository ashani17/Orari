using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orari.Models;

namespace Orari.DataDbContext
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Courses> Courses { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Enrollments> Enrollments { get; set; }
        public DbSet<Exams> Exams { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<StudyPrograms> StudyPrograms { get; set; }
        public DbSet<StudyProgramCourse> StudyProgramCourses { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<RecurringSchedule> RecurringSchedules { get; set; }
        public DbSet<ScheduleException> ScheduleExceptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure optional one-to-one relationship between Exams and Schedules
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Schedule)
                .WithOne(s => s.Exam)
                .HasForeignKey<Schedules>(s => s.EId)
                .IsRequired(false);  // Make the relationship optional

            // Configure Exam-Course relationship using CId
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CId);

            // Configure Exam-Professor relationship using ProfessorId
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Professor)
                .WithMany()
                .HasForeignKey(e => e.ProfessorId)
                .IsRequired(false);  // Make the relationship optional

            // Configure Exam-Room relationship using RId
            modelBuilder.Entity<Exams>()
                .HasOne(e => e.Room)
                .WithMany()
                .HasForeignKey(e => e.RId);

            // Configure Schedule-Professor relationship using ProfessorId
            modelBuilder.Entity<Schedules>()
                .HasOne(s => s.Professor)
                .WithMany()
                .HasForeignKey(s => s.ProfessorId)
                .IsRequired(false);  // Make the relationship optional

            modelBuilder.Entity<StudyProgramCourse>()
                .HasKey(spc => spc.Id); // Or composite key if applicable

            modelBuilder.Entity<StudyProgramCourse>()
                .HasOne(spc => spc.StudyProgram)
                .WithMany(sp => sp.StudyProgramCourse)
                .HasForeignKey(spc => spc.SPId);

            modelBuilder.Entity<StudyProgramCourse>()
                .HasOne(spc => spc.Course)
                .WithMany(c => c.StudyProgramCourse)
                .HasForeignKey(spc => spc.CId);

            modelBuilder.Entity<StudyPrograms>()
                .HasOne(sp => sp.Departments)
                .WithMany(d => d.StudyPrograms)
                .HasForeignKey(sp => sp.DId);

            // Configure Enrollments-User relationship (instead of Students)
            modelBuilder.Entity<Enrollments>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId);
        }
    }
}
