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

        public Ciudad Ciudad { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; } = null!; // Podríamos ocultarla parcialmente en la vista pública

        public TipoPropiedad TipoPropiedad { get; set; }

        public TipoOperacion TipoOperacion { get; set; }

        public EstadoPropiedad Estado { get; set; } = EstadoPropiedad.Disponible;

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

        // Relaciones
        public List<ImagenPropiedad> Imagenes { get; set; } = new List<ImagenPropiedad>();
    }
}