using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/clientes")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClienteController(IClienteRepository repo) : ControllerBase
{
    private readonly IClienteRepository _repo = repo;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            Cliente? cliente = await _repo.GetByIdAsync(id);

            if (cliente != null)
                return Ok(new { data = ClienteVM.From(cliente) });
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
    public async Task<IActionResult> Get([FromQuery] FiltrosCliente filters)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        List<Cliente> clientes = [];
        try
        {
            int cantidadClientes = await _repo.CountAsync(filters);
            if (cantidadClientes > 0)
                clientes = await _repo.ListAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadClientes / filters.Limit);

            return Ok(new { 
                clientes = ClienteVM.FromList(clientes), 
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
    public async Task<IActionResult> Post([FromBody] ClienteVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Cliente cliente = Cliente.From(vm);
        if (string.IsNullOrWhiteSpace(cliente.Telefono))
            cliente.Telefono = null;
        if (string.IsNullOrWhiteSpace(cliente.Direccion))
            cliente.Direccion = null;
        if (string.IsNullOrWhiteSpace(cliente.Cuit))
            cliente.Cuit = null;

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
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ClienteVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        vm.Id = id;
        if (string.IsNullOrWhiteSpace(vm.Telefono))
            vm.Telefono = null;
        if (string.IsNullOrWhiteSpace(vm.Direccion))
            vm.Direccion = null;
        if (string.IsNullOrWhiteSpace(vm.Cuit))
            vm.Cuit = null;

        try
        {
            if (await _repo.UpdateAsync(Cliente.From(vm)))
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
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Delete([FromRoute] int id, [FromQuery] FiltrosCliente filters)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        try
        {
            if (await _repo.DeleteAsync(id))
            {
                if (filters.ReturnUpdated)
                {
                    var clientes = await _repo.ListAsync(filters);
                    return Ok(new { listaActualizada = ClienteVM.FromList(clientes) });
                } 
                else
                {
                    return Ok();
                }
            }
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