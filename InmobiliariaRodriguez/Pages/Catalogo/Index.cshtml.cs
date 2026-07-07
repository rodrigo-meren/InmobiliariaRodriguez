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

        // Propiedades vinculadas a los filtros
        [BindProperty(SupportsGet = true)] public string? Partido { get; set; }
        [BindProperty(SupportsGet = true)] public string? Ciudad { get; set; } // Nuevo filtro
        [BindProperty(SupportsGet = true)] public string? Operacion { get; set; }
        [BindProperty(SupportsGet = true)] public string? Tipo { get; set; }
        [BindProperty(SupportsGet = true)] public decimal? PrecioMax { get; set; }

        // Lista para llenar el desplegable de localidades automáticamente
        public List<string> CiudadesDisponibles { get; set; } = new List<string>();

        public async Task OnGetAsync()
        {
            var query = _context.Propiedades.Include(p => p.Imagenes).AsQueryable();

            // 1. Filtro estricto por Partido (Ahora es invisible para el usuario)
            if (!string.IsNullOrEmpty(Partido) && Enum.TryParse<Partido>(Partido, out var partidoEnum))
            {
                query = query.Where(p => p.Partido == partidoEnum);
            }

            // MAGIA: Antes de seguir filtrando, buscamos qué ciudades hay disponibles en este partido
            CiudadesDisponibles = await query
                .Select(p => p.Ciudad)
                .Distinct()
                .ToListAsync();

            // 2. Filtro por Localidad/Barrio
            if (!string.IsNullOrEmpty(Ciudad))
            {
                query = query.Where(p => p.Ciudad == Ciudad);
            }

            // 3. Filtro por Operación
            if (!string.IsNullOrEmpty(Operacion) && Enum.TryParse<TipoOperacion>(Operacion, out var operacionEnum))
            {
                query = query.Where(p => p.TipoOperacion == operacionEnum);
            }

            // 4. Filtro por Tipo de Inmueble
            if (!string.IsNullOrEmpty(Tipo) && Enum.TryParse<TipoPropiedad>(Tipo, out var tipoEnum))
            {
                query = query.Where(p => p.TipoPropiedad == tipoEnum);
            }

            // 5. Filtro por Precio Máximo
            if (PrecioMax.HasValue && PrecioMax.Value > 0)
            {
                query = query.Where(p => p.Precio <= PrecioMax.Value);
            }

            Propiedades = await query.ToListAsync();
        }


    }
}