using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using InmobiliariaRodriguez.Web.Models.InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Usuarios
{
    public class CrearModel : PageModel
    {
        private readonly AppDbContext _context;

        public CrearModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario NuevoUsuario { get; set; } = default!;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Le decimos a .NET que no valide estos campos porque los llenamos nosotros en código
            ModelState.Remove("NuevoUsuario.NombreUsuario");
            ModelState.Remove("NuevoUsuario.Password");

            if (!ModelState.IsValid) return Page();

            // Automatización: El usuario y la clave inicial son el DNI
            NuevoUsuario.NombreUsuario = NuevoUsuario.Dni;
            NuevoUsuario.Password = NuevoUsuario.Dni;
            NuevoUsuario.RequiereCambioPassword = true; // Obligamos a que la cambie cuando entre

            _context.Usuarios.Add(NuevoUsuario);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}