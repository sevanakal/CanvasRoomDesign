using CanvasRoomDesign.ModelGeneral;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq.Expressions;

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
            base.OnModelCreating(modelBuilder);

            // Otomatik Filtreleme Mekanizması
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Eğer entity ISoftDelete interface'ini implemente ediyorsa
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(CreateIsDeletedFilter(entityType.ClrType));
                }
            }

            modelBuilder.Entity<Section>()
                .HasOne(s => s.hall)
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

        // Filtre oluşturucu yardımcı metod
        private static LambdaExpression CreateIsDeletedFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "it");
            var prop = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var body = Expression.Equal(prop, Expression.Constant(false));
            return Expression.Lambda(body, parameter);
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
