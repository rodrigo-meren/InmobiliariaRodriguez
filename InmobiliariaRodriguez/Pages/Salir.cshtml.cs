using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InmobiliariaRodriguez.Web.Pages
{
    public class SalirModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // 1. Destruye la credencial (Cookie) del navegador
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Lo manda de vuelta a la página principal pública
            return RedirectToPage("/Index");
        }
    }
}