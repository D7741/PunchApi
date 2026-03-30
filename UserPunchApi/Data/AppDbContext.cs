using Microsoft.EntityFrameworkCore;
using UserPunchApi.Models;
using EventManagementApi.Models;

namespace UserPunchApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 
        public DbSet<Department> Departments { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<PunchRecord> PunchRecords { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                        .HasOne(u => u.Department)
                        .WithMany(d => d.Users)
                        .HasForeignKey(u => u.DepartmentId)
                        .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<PunchRecord>()
                        .HasOne(p => p.User)
                        .WithMany(u => u.PunchRecords)
                        .HasForeignKey(p => p.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveRequest>()
                        .HasOne(l => l.User)
                        .WithMany(u => u.LeaveRequests)
                        .HasForeignKey(l => l.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                        .HasOne(s => s.User)
                        .WithMany(u => u.Schedules)
                        .HasForeignKey(s => s.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<PunchRecord>()
                        .HasIndex(p => new { p.UserId, p.PunchInTime })
                        .IsUnique();
            
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

        }
    }
}
