using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using InmobiliariaRodriguez.Web.Models.InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Usuarios
{
    public class EditarModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditarModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Usuario UsuarioActualizar { get; set; } = default!;

        [BindProperty]
        public bool ResetearClave { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userDb = await _context.Usuarios.FindAsync(id);
            if (userDb == null) return RedirectToPage("./Index");

            UsuarioActualizar = userDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Ignoramos la validación de estos campos
            ModelState.Remove("UsuarioActualizar.NombreUsuario");
            ModelState.Remove("UsuarioActualizar.Password");

            if (!ModelState.IsValid) return Page();

            var userDb = await _context.Usuarios.FindAsync(UsuarioActualizar.Id);
            if (userDb == null) return NotFound();

            // Actualizamos los datos personales
            userDb.Nombre = UsuarioActualizar.Nombre;
            userDb.Apellido = UsuarioActualizar.Apellido;
            userDb.Dni = UsuarioActualizar.Dni;

            // Si tildaron la cajita, le reseteamos la clave al DNI nuevo/actual
            if (ResetearClave)
            {
                userDb.Password = UsuarioActualizar.Dni;
                userDb.RequiereCambioPassword = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}