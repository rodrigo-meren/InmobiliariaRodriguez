using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaRodriguez.Web.Models
{
    public class Propiedad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150)]
        public string Titulo { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = null!;

        public Partido Partido { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(100)]
        public string Ciudad { get; set; } = null!;

        [StringLength(200)]
        public string Direccion { get; set; } = null!;

        public TipoPropiedad TipoPropiedad { get; set; }

        public TipoOperacion TipoOperacion { get; set; }

        
        public bool MostrarEnCatalogo { get; set; } = true;

        // Valores monetarios
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        public string Moneda { get; set; } = "USD"; // USD o ARS

        // Características físicas
        public int Ambientes { get; set; }
        public int Habitaciones { get; set; }
        public int Baños { get; set; }
        public int Cocheras { get; set; }
        public int MetrosTotales { get; set; }
        public int MetrosCubiertos { get; set; }

        // Banderas útiles para la UI
        public bool AptoCredito { get; set; }
        public bool EsDestacada { get; set; } // Para mostrar en el Home

        // Control interno
        public DateTime FechaAlta { get; set; } = DateTime.UtcNow;
        public EstadoPropiedad Estado { get; set; } = EstadoPropiedad.Disponible;
        // Relaciones
        public List<ImagenPropiedad> Imagenes { get; set; } = new List<ImagenPropiedad>();
    }
}