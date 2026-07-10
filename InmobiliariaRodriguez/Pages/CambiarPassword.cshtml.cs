using InmobiliariaRodriguez.Web.Datos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace InmobiliariaRodriguez.Web.Pages
{
    public class CambiarPasswordModel : PageModel
    {
        private readonly AppDbContext _context;

        public CambiarPasswordModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string NuevaPassword { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmarPassword { get; set; } = string.Empty;

        public string? MensajeError { get; set; }

        public IActionResult OnGet()
        {
            // Si intenta entrar acá sin pasar por el login primero, lo pateamos
            if (TempData.Peek("UsuarioIdCambio") == null)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Recuperamos el ID del usuario que pasamos desde el Login
            var usuarioIdObj = TempData["UsuarioIdCambio"];
            if (usuarioIdObj == null) return RedirectToPage("/Login");

            int usuarioId = (int)usuarioIdObj;

            if (NuevaPassword != ConfirmarPassword)
            {
                MensajeError = "Las contraseñas no coinciden.";
                TempData.Keep("UsuarioIdCambio"); // Mantenemos el ID para que no lo eche
                return Page();
            }

            // Buscamos el usuario y lo actualizamos
            var usuarioDb = await _context.Usuarios.FindAsync(usuarioId);
            if (usuarioDb != null)
            {
                usuarioDb.Password = NuevaPassword;
                usuarioDb.RequiereCambioPassword = false; // Ya no la tiene que cambiar

                await _context.SaveChangesAsync();

                // Ahora sí, le creamos la sesión oficial
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

            return RedirectToPage("/Login");
        }
    }
}