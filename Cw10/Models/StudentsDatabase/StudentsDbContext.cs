using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace Cw10.Models.StudentsDatabase
{
    public class StudentsDbContext : DbContext
    {
        private static IConfiguration _configuration;

        public StudentsDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public StudentsDbContext(DbContextOptions<StudentsDbContext> options, IConfiguration configuration) :
            base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleStudent> RoleStudents { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Study> Studies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer(_configuration["DatabaseConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.IdEnrollment).HasName("PK__Enrollme__5EBB800F546071E3");

                entity.ToTable("Enrollment");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.IdStudyNavigation)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.IdStudy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Enrollment_Studies");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole).HasName("PK__Role__B4369054A2EF3A71");

                entity.ToTable("Role");

                entity.HasIndex(e => e.RoleName, "UQ__Role__8A2B61606FBBB3BE").IsUnique();

                entity.Property(e => e.RoleName).IsRequired();
            });

            modelBuilder.Entity<RoleStudent>(entity =>
            {
                entity.HasKey(e => new {e.IdRole, e.IdStudent}).HasName("RoleStudent_PK");

                entity.ToTable("RoleStudent");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.RoleStudents)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RoleStudent_Role");

                entity.HasOne(d => d.IdStudentNavigation)
                    .WithMany(p => p.RoleStudents)
                    .HasForeignKey(d => d.IdStudent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RoleStudent_Student");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.IdStudent).HasName("PK__Student__61B351048E98812A");

                entity.ToTable("Student");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FirstName).IsRequired();

                entity.Property(e => e.IndexNumber).IsRequired();

                entity.Property(e => e.LastName).IsRequired();

                entity.Property(e => e.SaltPasswordHash).IsRequired();

                entity.HasOne(d => d.IdEnrollmentNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.IdEnrollment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Student_Enrollment");
            });

            modelBuilder.Entity<Study>(entity =>
            {
                entity.HasKey(e => e.IdStudy).HasName("PK__Studies__2B1257D3E99AF343");

                entity.Property(e => e.Name).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        private static void OnModelCreatingPartial(ModelBuilder modelBuilder) { }
    }
}