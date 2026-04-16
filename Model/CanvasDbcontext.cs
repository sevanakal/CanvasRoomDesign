using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CanvasRoomDesign.Model
{
    public class CanvasDbcontext : DbContext
     {
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<HallItem> HallItems { get; set; }

        public CanvasDbcontext(DbContextOptions<CanvasDbcontext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Hall)
                .WithMany(h => h.Sections)
                .HasForeignKey(s => s.HallId);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.Section)
                .WithMany(s => s.Groups)
                .HasForeignKey(g => g.SectionId);

            modelBuilder.Entity<HallItem>()
                .HasOne(hi => hi.Section)
                .WithMany(s => s.HallItems) // Section içine ICollection<HallItem> eklemeyi unutma
                .HasForeignKey(hi => hi.SectionId)
                .OnDelete(DeleteBehavior.Cascade); // Bölüm silinirse içindeki her şey (koltuk, kapı) gitsin.

            // 2. HallItem -> Group (Esnek Bağ)
            modelBuilder.Entity<HallItem>()
                .HasOne(hi => hi.Group)
                .WithMany(g => g.HallItems)
                .HasForeignKey(hi => hi.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

        }

        public class CanvasDbcontextFactory : IDesignTimeDbContextFactory<CanvasDbcontext>
        {
            public CanvasDbcontext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<CanvasDbcontext>();

                // Şifren ve sunucun (appsettings ile aynı)
                var connectionString = "Server=localhost;Port=3306;Database=HallMaster;Uid=root;Pwd=794613;";

                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

                return new CanvasDbcontext(optionsBuilder.Options);
            }
        }


    }
}
