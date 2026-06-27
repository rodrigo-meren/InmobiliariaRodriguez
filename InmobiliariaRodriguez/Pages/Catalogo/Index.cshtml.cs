using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaRodriguez.Web.Pages.Catalogo
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Propiedad> Propiedades { get; set; } = default!;

        // Guardamos el partido actual para mostrarlo en el título de la página
        public Partido? PartidoActual { get; set; }

        public async Task OnGetAsync(Partido? partido)
        {
            PartidoActual = partido;

            // Iniciamos la consulta trayendo solo las propiedades disponibles y cargando sus imágenes
            var consulta = _context.Propiedades
                .Include(p => p.Imagenes)
                .Where(p => p.Estado == EstadoPropiedad.Disponible);

            // Si llegó un partido por la URL (ej: hicieron clic en La Costa), filtramos
            if (partido.HasValue)
            {
                consulta = consulta.Where(p => p.Partido == partido.Value);
            }

            // Ejecutamos la consulta ordenando por las más recientes
            Propiedades = await consulta.OrderByDescending(p => p.FechaAlta).ToListAsync();
        }
    }
}