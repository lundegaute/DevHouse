using DevHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace DevHouse.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Project>(entity => {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.HasOne(p => p.Team)
                      .WithMany(t => t.Projects)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(p => p.ProjectType)
                      .WithMany(p => p.Projects)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Developer>(entity => {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FirstName).IsRequired();
                entity.Property(d => d.LastName).IsRequired();
                entity.HasOne(d => d.Team)
                      .WithMany(d => d.Developers)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Role)
                      .WithMany(d => d.Developers)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Team>(entity => {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired();
            });
            modelBuilder.Entity<Role>(entity => {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired();
            });
            modelBuilder.Entity<ProjectType>(entity => {
                entity.HasKey(pt => pt.Id);
                entity.Property(pt => pt.Name).IsRequired();
            });
        }
    }
}