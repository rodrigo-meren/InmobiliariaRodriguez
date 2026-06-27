using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Propiedades
{
    public class EliminarModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EliminarModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var propiedadDb = await _context.Propiedades.FirstOrDefaultAsync(p => p.Id == id);

            if (propiedadDb == null)
            {
                return RedirectToPage("./Index");
            }

            Propiedad = propiedadDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            // Buscamos la propiedad incluyendo sus imágenes
            var propiedadDb = await _context.Propiedades
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (propiedadDb != null)
            {
                // 1. Borramos las imágenes físicas del servidor
                if (propiedadDb.Imagenes != null && propiedadDb.Imagenes.Any())
                {
                    foreach (var imagen in propiedadDb.Imagenes)
                    {
                        // Extraemos solo el nombre del archivo de la URL
                        string nombreArchivo = Path.GetFileName(imagen.UrlImagen);
                        string rutaFisicaCompleta = Path.Combine(_webHostEnvironment.WebRootPath, "img", "propiedades", nombreArchivo);

                        // Si el archivo existe en el disco, lo eliminamos
                        if (System.IO.File.Exists(rutaFisicaCompleta))
                        {
                            System.IO.File.Delete(rutaFisicaCompleta);
                        }
                    }
                }

                // 2. Borramos la propiedad de la base de datos (las filas de imágenes se borran solas por la relación)
                _context.Propiedades.Remove(propiedadDb);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}