using InmobiliariaRodriguez.Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace InmobiliariaRodriguez.Web.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablas de la base de datos
        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<ImagenPropiedad> ImagenesPropiedades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos la relación Uno a Muchos entre Propiedad e Imagen
            modelBuilder.Entity<ImagenPropiedad>()
                .HasOne(i => i.Propiedad)
                .WithMany(p => p.Imagenes)
                .HasForeignKey(i => i.PropiedadId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra la propiedad, se borran sus fotos

            // Hacemos que el Email del usuario sea único para evitar cuentas duplicadas
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuramos decimal para el Precio de la propiedad
            modelBuilder.Entity<Propiedad>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        }
    }
}