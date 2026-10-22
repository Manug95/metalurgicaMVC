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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            Material? material = await _repo.GetByIdAsync(id);

            if (material != null)
                return Ok(new { data = MaterialVM.From(material) });
            else
                return NotFound(new { mensaje = "El material solicitado no existe" });
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

    [HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromBody] MaterialVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Material material = Material.From(vm);

        try
        {
            if ((await _repo.CreateAsync(material)) > 0)
            {
                vm.Id = material.Id;
                return Created($"api/materiales/{material.Id}", new { material = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el material" });
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
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] MaterialVM vm)
    {
        if (id <= 0)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var material = await _repo.GetByIdAsync(id);
            if (material == null)
                return NotFound();
            
            material.UpdateFrom(vm);
            vm.Id = id;

            if (await _repo.UpdateAsync(material))
                return Ok(new { material = vm });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el material" });
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
            return BadRequest();

        try
        {
            if (await _repo.DeleteAsync(id))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo borrar el material" });
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