using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Propiedades
{
    public class EditarModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditarModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = default!;

        // Lista para recibir fotos nuevas
        [BindProperty]
        public List<IFormFile> NuevasImagenes { get; set; } = new List<IFormFile>();

        // Lista para recibir los IDs de las fotos que el usuario tildó para borrar
        [BindProperty]
        public List<int> ImagenesAEliminar { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Traemos la propiedad con sus imágenes actuales
            var propiedadDb = await _context.Propiedades
                                            .Include(p => p.Imagenes)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            if (propiedadDb == null) return RedirectToPage("./Index");

            Propiedad = propiedadDb;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Limpiamos todo lo que no queremos que valide automáticamente
            ModelState.Remove("Propiedad.Imagenes");
            ModelState.Remove("Propiedad.FechaAlta");
            ModelState.Remove("NuevasImagenes");
            ModelState.Remove("ImagenesAEliminar");

            if (!ModelState.IsValid)
            {
                // Si falla, volvemos a cargar las imágenes de la BD para que la vista no explote
                var propDb = await _context.Propiedades
                                           .Include(p => p.Imagenes)
                                           .FirstOrDefaultAsync(p => p.Id == Propiedad.Id);
                if (propDb != null)
                {
                    Propiedad.Imagenes = propDb.Imagenes;
                }
                return Page();
            }

            var propiedadActualizar = await _context.Propiedades
                                                    .Include(p => p.Imagenes)
                                                    .FirstOrDefaultAsync(p => p.Id == Propiedad.Id);

            if (propiedadActualizar == null) return NotFound();



            // 1. Actualizamos datos de texto
            propiedadActualizar.Titulo = Propiedad.Titulo;
            propiedadActualizar.Descripcion = Propiedad.Descripcion;
            propiedadActualizar.Partido = Propiedad.Partido;
            propiedadActualizar.Ciudad = Propiedad.Ciudad;
            propiedadActualizar.Direccion = Propiedad.Direccion;
            propiedadActualizar.TipoOperacion = Propiedad.TipoOperacion;
            propiedadActualizar.TipoPropiedad = Propiedad.TipoPropiedad;
            propiedadActualizar.Moneda = Propiedad.Moneda;
            propiedadActualizar.Precio = Propiedad.Precio;
            propiedadActualizar.Ambientes = Propiedad.Ambientes;
            propiedadActualizar.Habitaciones = Propiedad.Habitaciones;
            propiedadActualizar.Baños = Propiedad.Baños;
            propiedadActualizar.Cocheras = Propiedad.Cocheras;
            propiedadActualizar.MetrosTotales = Propiedad.MetrosTotales;
            propiedadActualizar.MetrosCubiertos = Propiedad.MetrosCubiertos;
            propiedadActualizar.Estado = Propiedad.Estado;

            // Actualizamos el nuevo flag
            propiedadActualizar.MostrarEnCatalogo = Propiedad.MostrarEnCatalogo;

            // 2. Procesamos las imágenes a ELIMINAR
            if (ImagenesAEliminar.Any())
            {
                foreach (var idImagen in ImagenesAEliminar)
                {
                    var imgBorrar = propiedadActualizar.Imagenes.FirstOrDefault(i => i.Id == idImagen);
                    if (imgBorrar != null)
                    {
                        // Borramos del disco
                        string rutaFisica = Path.Combine(_webHostEnvironment.WebRootPath, "img", "propiedades", Path.GetFileName(imgBorrar.UrlImagen));
                        if (System.IO.File.Exists(rutaFisica)) System.IO.File.Delete(rutaFisica);

                        // Borramos de la BD
                        _context.Remove(imgBorrar);
                    }
                }
            }

            // 3. Procesamos las NUEVAS imágenes agregadas
            if (NuevasImagenes != null && NuevasImagenes.Count > 0)
            {
                string carpetaPropiedades = Path.Combine(_webHostEnvironment.WebRootPath, "img", "propiedades");

                // Si la propiedad se quedó sin imágenes (borraron todas), la primera nueva será portada
                bool noHayImagenesPrevias = !propiedadActualizar.Imagenes.Any(i => !ImagenesAEliminar.Contains(i.Id));

                foreach (var imagen in NuevasImagenes)
                {
                    if (imagen.Length > 0)
                    {
                        string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                        string rutaFisicaCompleta = Path.Combine(carpetaPropiedades, nombreArchivo);

                        using (var fileStream = new FileStream(rutaFisicaCompleta, FileMode.Create))
                        {
                            await imagen.CopyToAsync(fileStream);
                        }

                        propiedadActualizar.Imagenes.Add(new ImagenPropiedad
                        {
                            UrlImagen = "/img/propiedades/" + nombreArchivo,
                            EsPortada = noHayImagenesPrevias
                        });
                        noHayImagenesPrevias = false;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}