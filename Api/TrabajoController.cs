using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/trabajos")]
public class TrabajoController(ITrabajoRepository repo) : ControllerBase
{
    private readonly ITrabajoRepository _repo = repo;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        try
        {
            Trabajo? trabajo = await _repo.GetByIdAsync(id);

            if (trabajo != null)
                return Ok(new { data = TrabajoVM.From(trabajo) });
            else
                return NotFound(new { mensaje = "El trabajo solicitado no existe" });
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

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] FiltrosTrabajo filters)
    {
         try
        {
            List<Trabajo> trabajos = await _repo.ListAsync(filters);
            long cantidadTrabajos = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadTrabajos / filters.Limit);

            return Ok(new { 
                trabajos = TrabajoVM.FromList(trabajos), 
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

    [HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromBody] TrabajoVM vm)
    {
        Trabajo trabajo = Trabajo.From(vm);

        try
        {
            if ((await _repo.CreateAsync(trabajo)) > 0)
            {
                vm.Id = trabajo.Id;
                return Created($"api/trabajos/{trabajo.Id}", new { trabajo = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el trabajo" });
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

    [HttpPut("{id}")]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> Put([FromRoute] long id, [FromBody] TrabajoVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });
            
        try
        {
            Trabajo? trabajo = await _repo.GetByIdAsync(id);
            if (trabajo == null)
                return BadRequest(new { mensaje = "El empleado no existe" });

            trabajo.UpdateFrom(vm);

            if (await _repo.UpdateAsync(trabajo))
                return Ok(new { trabajo = TrabajoVM.From(trabajo) });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el empleado" });
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        try
        {
            if (await _repo.DeleteAsync(id))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo borrar el trabajo" });
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