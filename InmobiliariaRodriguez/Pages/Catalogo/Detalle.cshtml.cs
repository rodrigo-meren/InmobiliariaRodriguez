using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaRodriguez.Web.Pages.Catalogo
{
    public class DetalleModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetalleModel(AppDbContext context)
        {
            _context = context;
        }

        public Propiedad Propiedad { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Buscamos la propiedad específica y traemos su lista de imágenes
            var propiedadDb = await _context.Propiedades
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Si por algún motivo alguien escribe un ID que no existe en la URL, lo mandamos al inicio
            if (propiedadDb == null)
            {
                return RedirectToPage("/Index");
            }

            Propiedad = propiedadDb;
            return Page();
        }
    }
}