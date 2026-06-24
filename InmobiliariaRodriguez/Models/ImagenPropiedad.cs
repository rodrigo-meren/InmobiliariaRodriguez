using System.ComponentModel.DataAnnotations;

namespace InmobiliariaRodriguez.Web.Models
{
    public class ImagenPropiedad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UrlImagen { get; set; } = null!;

        public bool EsPortada { get; set; } // La que sale en la tarjeta principal

        // Relación con la propiedad
        public int PropiedadId { get; set; }
        public Propiedad Propiedad { get; set; } = null!;
    }
}