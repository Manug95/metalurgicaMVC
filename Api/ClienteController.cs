using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/clientes")]
public class ClienteController(IClienteRepository repo) : ControllerBase
{
    private readonly IClienteRepository _repo = repo;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCliente([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            Cliente? cliente = await _repo.GetByIdAsync(id);

            if (cliente != null)
                return Ok(new { data = ClienteVM.Parse(cliente) });
            else
                return NotFound(new { mensaje = "El cliente solicitado no existe" });
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
    public async Task<IActionResult> GetCliente([FromQuery] FiltrosCliente filters)
    {
        try
        {
            List<Cliente> clientes = await _repo.ListAsync(filters);
            int cantidadClientes = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadClientes / filters.Limit);

            return Ok(new { 
                clientes = ClienteVM.ParseList(clientes), 
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
    public async Task<IActionResult> PostCliente([FromBody] ClienteVM vm)
    {
        if (!ModelState.IsValid) 
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        Cliente cliente = Cliente.Parse(vm);

        try
        {
            if ((await _repo.CreateAsync(cliente)) > 0)
            {
                vm.Id = cliente.Id;
                return Created($"api/clientes/{cliente.Id}", new { cliente = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el cliente" });
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
    public async Task<IActionResult> PutCliente([FromRoute] int id, [FromBody] ClienteVM vm)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        vm.Id = id;

        try
        {
            if (await _repo.UpdateAsync(Cliente.Parse(vm)))
                return Ok(new { cliente = vm });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el cliente" });
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
    public async Task<IActionResult> DeleteCliente([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            if (await _repo.DeleteAsync(id))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo borrar el cliente" });
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