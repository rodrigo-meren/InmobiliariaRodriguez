using InmobiliariaRodriguez.Web.Datos;
using InmobiliariaRodriguez.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InmobiliariaRodriguez.Web.Pages.Admin.Propiedades
{
    public class CrearModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Nos permite acceder a wwwroot

        // Inyectamos el contexto de BD y el entorno web
        public CrearModel(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Propiedad Propiedad { get; set; } = new Propiedad();

        // Esta propiedad atrapa todos los archivos que suba el usuario
        [BindProperty]
        public List<IFormFile> ImagenesSubidas { get; set; } = new List<IFormFile>();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Lógica para procesar las imágenes subidas
            if (ImagenesSubidas != null && ImagenesSubidas.Count > 0)
            {
                // 1. Definimos la ruta de la carpeta física: wwwroot/img/propiedades
                string carpetaPropiedades = Path.Combine(_webHostEnvironment.WebRootPath, "img", "propiedades");

                // 2. Si la carpeta no existe, la creamos automáticamente
                if (!Directory.Exists(carpetaPropiedades))
                {
                    Directory.CreateDirectory(carpetaPropiedades);
                }

                bool esPrimeraImagen = true;

                // 3. Recorremos cada imagen que seleccionó el empleado
                foreach (var imagen in ImagenesSubidas)
                {
                    if (imagen.Length > 0)
                    {
                        // Generamos un nombre único usando Guid para que no se pisen si dos fotos se llaman "frente.jpg"
                        string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                        string rutaFisicaCompleta = Path.Combine(carpetaPropiedades, nombreArchivo);

                        // Copiamos la imagen al disco duro del servidor
                        using (var fileStream = new FileStream(rutaFisicaCompleta, FileMode.Create))
                        {
                            await imagen.CopyToAsync(fileStream);
                        }

                        // Generamos el registro para la Base de Datos
                        var imagenPropiedad = new ImagenPropiedad
                        {
                            UrlImagen = "/img/propiedades/" + nombreArchivo,
                            EsPortada = esPrimeraImagen // La primera foto seleccionada queda como portada
                        };

                        // Se la agregamos a la propiedad actual
                        Propiedad.Imagenes.Add(imagenPropiedad);
                        esPrimeraImagen = false;
                    }
                }
            }

            // Guardamos la propiedad (Entity Framework guarda automáticamente las imágenes asociadas)
            _context.Propiedades.Add(Propiedad);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Propiedades/Index");
        }
    }
}