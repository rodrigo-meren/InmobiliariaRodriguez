using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using InmobiliariaRodriguez.Web.Models.InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Usuarios
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Usuario> Usuarios { get; set; } = default!;

        public async Task OnGetAsync()
        {
            // Traemos todos los usuarios de la base de datos
            Usuarios = await _context.Usuarios.ToListAsync();
        }
    }
}