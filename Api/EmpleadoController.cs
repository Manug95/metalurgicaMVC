using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/empleados")]
public class EmpleadoController(IEmpleadoRepository repo) : ControllerBase
{
    private readonly IEmpleadoRepository _repo = repo;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmpleado([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        try
        {
            Empleado? empleado = await _repo.GetByIdAsync(id);

            if (empleado != null)
                return Ok(new { data = EmpleadoVM.From(empleado) });
            else
                return NotFound(new { mensaje = "El empleado solicitado no existe" });
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
    public async Task<IActionResult> GetEmpleado([FromQuery] FiltrosEmpleado filters)
    {
         try
        {
            List<Empleado> empleados = await _repo.ListAsync(filters);
            int cantidadEmpleados = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadEmpleados / filters.Limit);

            return Ok(new { 
                empleados = EmpleadoVM.FromList(empleados), 
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
    public async Task<IActionResult> PostEmpleado([FromBody] EmpleadoVM vm)
    {
        Empleado empleado = Empleado.From(vm);

        try
        {
            if ((await _repo.CreateAsync(empleado)) > 0)
            {
                vm.Id = empleado.Id;
                return Created($"api/empleados/{empleado.Id}", new { empleado = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el empleado" });
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
    public async Task<IActionResult> PutEmpleado([FromRoute] int id, [FromBody] EmpleadoVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });
            
        try
        {
            Empleado? empleado = await _repo.GetByIdAsync(id);
            if (empleado == null)
                return BadRequest(new { mensaje = "El empleado no existe" });

            empleado.UpdateFrom(vm);

            if (await _repo.UpdateAsync(empleado))
                return Ok(new { empleado = EmpleadoVM.From(empleado) });
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
    public async Task<IActionResult> DeleteEmpleado([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        try
        {
            if (await _repo.DeleteAsync(id))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo borrar el empleado" });
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