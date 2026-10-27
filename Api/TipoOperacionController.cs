using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/[controller]")]
public  class TipoOperacionController(ITipoOperacionRepository tipoOperacionRepository) : ControllerBase
{
    private readonly ITipoOperacionRepository _repo = tipoOperacionRepository;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "La id es incorrecta" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            TipoOperacion? tipoOperacion = await _repo.GetByIdAsync(id);

            if (tipoOperacion != null)
                return Ok(new { data = TipoOperacionVM.From(tipoOperacion) });
            else
                return NotFound(new { mensaje = "El tipo de operación solicitado no existe" });
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
    public async Task<IActionResult> Get([FromQuery] FiltrosTipoOperacion filters)
    {
        List<TipoOperacion> tiposOperaciones = [];
        try
        {
            int cantidadTiposOperaciones = await _repo.CountAsync(filters);
            if (cantidadTiposOperaciones > 0)
            {
                filters.Limit = cantidadTiposOperaciones;
                tiposOperaciones = await _repo.ListAsync(filters);
            }

            decimal cp = Math.Ceiling((decimal)cantidadTiposOperaciones / filters.Limit);

            return Ok(new { 
                tiposOperaciones = TipoOperacionVM.FromList(tiposOperaciones), 
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
    public async Task<IActionResult> Post([FromBody] TipoOperacionVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        TipoOperacion tipoOperacion = TipoOperacion.From(vm);

        try
        {
            if ((await _repo.CreateAsync(tipoOperacion)) > 0)
            {
                vm.Id = tipoOperacion.Id;
                return Created($"api/TipoOperacion/{tipoOperacion.Id}", new { tipoOperacion = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el tipo de operación" });
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
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TipoOperacionVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "La id es incorrecta" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var tipoOperacion = await _repo.GetByIdAsync(id);
            if (tipoOperacion == null)
                return NotFound(new { mensaje = "El tipo de operación no existe" });
            
            tipoOperacion.UpdateFrom(vm);
            vm.Id = id;

            if (await _repo.UpdateAsync(tipoOperacion))
                return Ok(new { tipoOperacion = vm });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el tipo de operación" });
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
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "La id es incorrecta" });

        try
        {
            if (await _repo.DeleteAsync(id))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo borrar el tipo de operación" });
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