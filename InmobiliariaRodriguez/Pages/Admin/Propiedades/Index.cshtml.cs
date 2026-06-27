using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Propiedades
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // Acá guardamos la lista de propiedades que viaja al HTML
        public IList<Propiedad> Propiedades { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Traemos todas las propiedades ordenadas desde la más nueva a la más vieja
            Propiedades = await _context.Propiedades
                                        .OrderByDescending(p => p.FechaAlta)
                                        .ToListAsync();
        }
    }
}