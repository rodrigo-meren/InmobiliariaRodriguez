using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Propiedades
{
    public class CrearModel : PageModel
    {
        private readonly AppDbContext _context;

        public CrearModel(AppDbContext context)
        {
            _context = context;
        }

        // El [BindProperty] conecta automáticamente los inputs del HTML con este objeto
        [BindProperty]
        public Propiedad Propiedad { get; set; } = new Propiedad();

        public void OnGet()
        {
            // Esta función se ejecuta cuando el empleado entra a la página para ver el formulario
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validamos que todos los campos requeridos estén completos
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Guardamos la propiedad en la base de datos
            _context.Propiedades.Add(Propiedad);
            await _context.SaveChangesAsync();

            // Después de guardar, lo mandamos al listado (que vamos a crear luego)
            return RedirectToPage("/Admin/Propiedades/Index");
        }
    }
}