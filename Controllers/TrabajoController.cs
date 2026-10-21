using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

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

        try
        {
            List<Trabajo> trabajos = await _repo.ListAsync(filters);
            long cantidadTrabajos = await _repo.CountAsync(filters);

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
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los trabajos";
        }

        return View(new List<TrabajoVM>());
    }
    
    public async Task<IActionResult> Create([FromServices] IMaterialRepository materialRepository, [FromServices] ITipoOperacionRepository tipoOperacionRepository)
    {
        var filtersMaterial = new FiltrosMaterial();
        var materiales = await materialRepository.ListAsync(filtersMaterial);
        var cantidadMateriales = await materialRepository.CountAsync(filtersMaterial);
        decimal cantidadPaginasMateriales = Math.Ceiling((decimal)cantidadMateriales / filtersMaterial.Limit);
        var tiposOperacion = await tipoOperacionRepository.ListAsync(new FiltrosTipoOperacion(){ Limit = 0 });
        var trabajo = new TrabajoVM(){ Estado = "" };

        return View(
            new FormTrabajoVM(trabajo, materiales, tiposOperacion) 
            { 
                CantidadPaginasMateriales = (int)(cantidadPaginasMateriales <= 0 ? 1 : cantidadPaginasMateriales) 
            }
        );
    }
    
    public async Task<IActionResult> Edit([FromRoute] long id, [FromServices] IMaterialRepository materialRepository, [FromServices] ITipoOperacionRepository tipoOperacionRepository)
    {
        var filtersMaterial = new FiltrosMaterial();
        Trabajo? trabajo = null;
        List<Material> materiales = [];
        List<TipoOperacion> tiposOperacion = [];
        int cantidadMateriales;
        decimal cantidadPaginasMateriales = 0m;

        try
        {
            materiales = await materialRepository.ListAsync(filtersMaterial);
            cantidadMateriales = await materialRepository.CountAsync(filtersMaterial);
            cantidadPaginasMateriales = Math.Ceiling((decimal)cantidadMateriales / filtersMaterial.Limit);
            tiposOperacion = await tipoOperacionRepository.ListAsync(new FiltrosTipoOperacion(){ Limit = 0 });
            trabajo = await _repo.GetByIdAsync(id);
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
            new FormTrabajoVM(trabajo != null ? TrabajoVM.From(trabajo) : new TrabajoVM(), materiales, tiposOperacion) 
            { 
                CantidadPaginasMateriales = (int)(cantidadPaginasMateriales <= 0 ? 1 : cantidadPaginasMateriales) 
            }
        );
    }

    [HttpGet]
    public async Task<IActionResult> FotosForm([FromRoute] long id, [FromServices] IFotoRepository repoFotos)
    {
        List<Foto> fotos = [];

        try
        {
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