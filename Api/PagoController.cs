using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/pagos")]
public class PagoController(IPagoRepository repo): ControllerBase
{
    private readonly IPagoRepository _repo = repo;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        try
        {
            Pago? pago = await _repo.GetByIdAsync(id);

            if (pago != null)
                return Ok(new { data = PagoVM.From(pago) });
            else
                return NotFound(new { mensaje = "El pago solicitado no existe" });
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
    public async Task<IActionResult> Get([FromQuery] FiltrosPago filters)
    {
        try
        {
            List<Pago> pagos = await _repo.ListAsync(filters);
            long cantidadPagos = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadPagos / filters.Limit);

            return Ok(new { 
                pagos = PagoVM.FromList(pagos), 
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
    public async Task<IActionResult> Post([FromBody] PagoVM vm)
    {
        Pago pago = Pago.From(vm);

        try
        {
            if ((await _repo.CreateAsync(pago)) > 0)
            {
                vm.Id = pago.Id;
                return Created($"api/pagos/{pago.Id}", new { pago = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el pago" });
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
    public async Task<IActionResult> Put([FromRoute] long id, [FromBody] PagoVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });
            
        try
        {
            Pago? pago = await _repo.GetByIdAsync(id);
            if (pago == null)
                return BadRequest(new { mensaje = "El pago no existe" });

            pago.UpdateFrom(vm);

            if (await _repo.UpdateAsync(pago))
                return Ok(new { pago = PagoVM.From(pago) });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el pago" });
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
                return BadRequest(new { mensaje = "No se pudo borrar el pago" });
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