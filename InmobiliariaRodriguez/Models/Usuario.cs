using System.ComponentModel.DataAnnotations;

namespace InmobiliariaRodriguez.Web.Models
{
    namespace InmobiliariaRodriguez.Web.Models
    {
        public class Usuario
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
            public string Apellido { get; set; } = null!;
            public string Dni { get; set; } = null!;

            public string NombreUsuario { get; set; } = null!;
            public string Password { get; set; } = null!;

            // Esta bandera nos dice si el usuario todavía tiene la clave por defecto
            public bool RequiereCambioPassword { get; set; } = true;
        }
    }
}