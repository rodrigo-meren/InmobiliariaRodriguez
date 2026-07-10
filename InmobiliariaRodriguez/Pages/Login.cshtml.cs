using InmobiliariaRodriguez.Web.Datos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InmobiliariaRodriguez.Web.Pages
{
    public class LoginModel : PageModel
    {
        // Variable para manejar la base de datos
        private readonly AppDbContext _context;

        // Constructor que inyecta la base de datos
        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Usuario { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? MensajeError { get; set; }

        public void OnGet()
        {
            // Página de carga inicial
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Buscamos al usuario en la base de datos
            var usuarioDb = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == Usuario && u.Password == Password);

            if (usuarioDb != null)
            {
                // ¿Es su primer ingreso? Lo mandamos a cambiar la clave
                if (usuarioDb.RequiereCambioPassword)
                {
                    TempData["UsuarioIdCambio"] = usuarioDb.Id;
                    return RedirectToPage("/CambiarPassword");
                }

                // Si ya cambió la clave, creamos su credencial (cookie)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioDb.NombreUsuario),
                    new Claim("NombreCompleto", $"{usuarioDb.Nombre} {usuarioDb.Apellido}")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToPage("/Admin/Propiedades/Index");
            }

            // Si los datos no coinciden
            MensajeError = "Usuario o contraseña incorrectos.";
            return Page();
        }
    }
}