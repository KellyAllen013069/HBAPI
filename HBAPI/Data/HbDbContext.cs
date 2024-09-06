using Microsoft.EntityFrameworkCore;
using HBAPI.Models;

namespace HBAPI.Data
{
    public class HbDbContext : DbContext
    {
        public HbDbContext(DbContextOptions<HbDbContext> options)
            : base(options)
        {
        }

        public DbSet<DanceClass> DanceClasses { get; set; }
        public DbSet<ClassDay> ClassDays { get; set; }
        public DbSet<ClassLevel> ClassLevels { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Classes_Days relationship
            modelBuilder.Entity<ClassDay>()
                .HasOne(cd => cd.DanceClass)
                .WithMany(dc => dc.ClassDays)
                .HasForeignKey(cd => cd.ClassId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Configure cascade delete behavior

            modelBuilder.Entity<ClassDay>()
                .HasOne(cd => cd.Day)
                .WithMany(d => d.ClassDays)
                .HasForeignKey(cd => cd.DayId)
                .OnDelete(DeleteBehavior.Cascade);  

            // Configure Classes_Levels relationship
            modelBuilder.Entity<ClassLevel>()
                .HasOne(cl => cl.DanceClass)
                .WithMany(dc => dc.ClassLevels)
                .HasForeignKey(cl => cl.ClassId)
                .OnDelete(DeleteBehavior.Cascade);  
            
            modelBuilder.Entity<ClassLevel>()
                .HasOne(cl => cl.Level)
                .WithMany(l => l.ClassLevels)
                .HasForeignKey(cl => cl.LevelId)
                .OnDelete(DeleteBehavior.Cascade);  
            
        }
    }
}