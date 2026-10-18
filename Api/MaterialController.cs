using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/materiales")]
public class MaterialController(IMaterialRepository materialRepository) : ControllerBase
{
    private readonly IMaterialRepository _repo = materialRepository;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FiltrosMaterial filters)
    {
        try
        {
            List<Material> materiales = await _repo.ListAsync(filters);
            long cantidadMateriales = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadMateriales / filters.Limit);

            return Ok(new { 
                materiales = MaterialVM.FromList(materiales), 
                cantidadPaginas = cp <= 0 ? 1 : cp 
            });
        }
        catch (ClientException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (AppException)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
    }
}