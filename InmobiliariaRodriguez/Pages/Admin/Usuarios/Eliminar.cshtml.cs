using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using InmobiliariaRodriguez.Web.Models.InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Usuarios
{
    public class EliminarModel : PageModel
    {
        private readonly AppDbContext _context;

        public EliminarModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userDb = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            // Si no existe o es el admin principal, lo pateamos
            if (userDb == null || userDb.Id == 1) return RedirectToPage("./Index");

            Usuario = userDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userDb = await _context.Usuarios.FindAsync(id);

            if (userDb != null && userDb.Id != 1)
            {
                _context.Usuarios.Remove(userDb);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}