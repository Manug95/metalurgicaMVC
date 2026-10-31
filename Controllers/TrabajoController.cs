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
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los trabajos";
        }

        return View(TrabajoVM.FromList(trabajos));
    }
    
    [HttpGet]
    public async Task<IActionResult> Create([FromServices] IMaterialRepository materialRepository, [FromServices] ITipoOperacionRepository tipoOperacionRepository)
    {
        FiltrosMaterial filtersMaterial = new();
        List<Material> materiales = [];
        List<TipoOperacion> tiposOperacion = [];
        int cantidadMateriales;
        decimal cantidadPaginasMateriales = 0m;
        try
        {
            cantidadMateriales = await materialRepository.CountAsync(filtersMaterial);
            if (cantidadMateriales > 0)
                materiales = await materialRepository.ListAsync(filtersMaterial);
            cantidadPaginasMateriales = Math.Ceiling((decimal)cantidadMateriales / filtersMaterial.Limit);
            tiposOperacion = await tipoOperacionRepository.ListAsync(new FiltrosTipoOperacion(){ Limit = 0 });
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los trabajos";
        }

        return View(
            new FormTrabajoVM(new TrabajoVM(){ Estado = "" }, materiales, tiposOperacion) 
            { 
                CantidadPaginasMateriales = (int)(cantidadPaginasMateriales <= 0 ? 1 : cantidadPaginasMateriales) 
            }
        );
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] long id, [FromServices] IMaterialRepository materialRepository, [FromServices] ITipoOperacionRepository tipoOperacionRepository)
    {
        if (id <= 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", "La ID del trabajo es incorrecta");
        }
        
        var filtrosMaterial = new FiltrosMaterial();
        Trabajo? trabajo = new();
        List<Material> materiales = [];
        List<TipoOperacion> tiposOperacion = [];
        int cantidadMateriales;
        decimal cantidadPaginasMateriales = 0m;

        try
        {
            trabajo = await _repo.GetByIdAsync(id);
            if (trabajo == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("NotFound");
            }
            cantidadMateriales = await materialRepository.CountAsync(filtrosMaterial);
            if (cantidadMateriales > 0)
                materiales = await materialRepository.ListAsync(filtrosMaterial);
            cantidadPaginasMateriales = Math.Ceiling((decimal)cantidadMateriales / filtrosMaterial.Limit);
            tiposOperacion = await tipoOperacionRepository.ListAsync(new FiltrosTipoOperacion(){ Limit = 0 });
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los trabajos";
        }

        return View("Create", 
            new FormTrabajoVM(TrabajoVM.From(trabajo ?? new Trabajo()), materiales, tiposOperacion) 
            { 
                CantidadPaginasMateriales = (int)(cantidadPaginasMateriales <= 0 ? 1 : cantidadPaginasMateriales) 
            }
        );
    }

    [HttpGet]
    public async Task<IActionResult> FotosForm([FromRoute] long id, [FromServices] IFotoRepository repoFotos)
    {
        if (id <= 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", "La ID del trabajo es incorrecta");
        }
        List<Foto> fotos = [];
        try
        {
            Trabajo? trabajo = await _repo.GetByIdAsync(id);
            if (trabajo == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("NotFound");
            }
            fotos = await repoFotos.ListAsync(new FiltrosFoto() { TrabajoId = id });
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer las fotos";
        }

        return View(new FotoVM()
        {
            TrabajoId = id,
            Fotos = fotos
        });
    }
}