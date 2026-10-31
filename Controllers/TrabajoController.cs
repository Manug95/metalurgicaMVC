using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

[Authorize]
public class TrabajoController(ITrabajoRepository repo) : Controller
{
    private readonly ITrabajoRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FiltrosTrabajo filters)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<TrabajoVM>());
        }

        List<Trabajo> trabajos = [];
        try
        {
            long cantidadTrabajos = await _repo.CountAsync(filters);
            if (cantidadTrabajos > 0)
                trabajos = await _repo.ListAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadTrabajos / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.busqueda = "";
            ViewBag.linkActivo = "trabajos";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(TrabajoVM.FromList(trabajos));
        }
        catch (ClientException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", ex.Message);
        }
        catch (AppException ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("InternalServerError", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Create([FromServices] IMaterialRepository materialRepository, [FromServices] ITipoOperacionRepository tipoOperacionRepository)
    {
        try
        {
            FiltrosMaterial filtersMaterial = new();
            List<Material> materiales = [];
            List<TipoOperacion> tiposOperacion = [];
            
            int cantidadMateriales = await materialRepository.CountAsync(filtersMaterial);
            if (cantidadMateriales > 0)
                materiales = await materialRepository.ListAsync(filtersMaterial);
            decimal cantidadPaginasMateriales = Math.Ceiling((decimal)cantidadMateriales / filtersMaterial.Limit);
            tiposOperacion = await tipoOperacionRepository.ListAsync(new FiltrosTipoOperacion(){ Limit = 0 });

            return View(
                new FormTrabajoVM(new TrabajoVM(){ Estado = "" }, materiales, tiposOperacion) 
                { 
                    CantidadPaginasMateriales = (int)(cantidadPaginasMateriales <= 0 ? 1 : cantidadPaginasMateriales) 
                }
            );
        }
        catch (ClientException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", ex.Message);
        }
        catch (AppException ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("InternalServerError", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] long id, [FromServices] IMaterialRepository materialRepository, [FromServices] ITipoOperacionRepository tipoOperacionRepository)
    {
        if (id <= 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", "La ID del trabajo es incorrecta");
        }    

        try
        {
            var filtrosMaterial = new FiltrosMaterial();
            List<Material> materiales = [];
            List<TipoOperacion> tiposOperacion = [];

            Trabajo? trabajo = await _repo.GetByIdAsync(id);
            if (trabajo == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("NotFound");
            }

            int cantidadMateriales = await materialRepository.CountAsync(filtrosMaterial);
            if (cantidadMateriales > 0)
                materiales = await materialRepository.ListAsync(filtrosMaterial);
            decimal cantidadPaginasMateriales = Math.Ceiling((decimal)cantidadMateriales / filtrosMaterial.Limit);
            tiposOperacion = await tipoOperacionRepository.ListAsync(new FiltrosTipoOperacion(){ Limit = 0 });

            return View("Create", 
                new FormTrabajoVM(TrabajoVM.From(trabajo), materiales, tiposOperacion) 
                { 
                    CantidadPaginasMateriales = (int)(cantidadPaginasMateriales <= 0 ? 1 : cantidadPaginasMateriales) 
                }
            );
        }
        catch (ClientException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", ex.Message);
        }
        catch (AppException ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("InternalServerError", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> FotosForm([FromRoute] long id, [FromServices] IFotoRepository repoFotos)
    {
        if (id <= 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", "La ID del trabajo es incorrecta");
        }

        try
        {
            Trabajo? trabajo = await _repo.GetByIdAsync(id);
            if (trabajo == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("NotFound");
            }
            List<Foto> fotos = await repoFotos.ListAsync(new FiltrosFoto() { TrabajoId = id });

            return View(new FotoVM()
            {
                TrabajoId = id,
                Fotos = fotos ?? []
            });
        }
        catch (ClientException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", ex.Message);
        }
        catch (AppException ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("InternalServerError", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
}