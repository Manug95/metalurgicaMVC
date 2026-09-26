using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels.UsuarioViewModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/usuarios")]
public class UsuarioController(IUsuarioRepository repo, IConfiguration config) : ControllerBase
{
    private readonly IUsuarioRepository _repo = repo;
    private readonly IConfiguration _config = config;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUsuario([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);

            if (usuario != null)
                return Ok(new { data = UsuarioVM.From(usuario) });
            else
                return NotFound(new { mensaje = "El usuario solicitado no existe" });
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
    public async Task<IActionResult> GetUsuario([FromQuery] FiltrosUsuario filters)
    {
         try
        {
            List<Usuario> usuarios = await _repo.ListAsync(filters);
            int cantidadUsuarios = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadUsuarios / filters.Limit);

            return Ok(new { 
                usuarios = UsuarioVM.FromList(usuarios), 
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
    public async Task<IActionResult> PostUsuario([FromBody] UsuarioVM vm)
    {
        if (!ModelState.IsValid) 
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        Usuario usuario = Usuario.From(vm);

        try
        {
            if ((await _repo.CreateAsync(usuario)) > 0)
            {
                vm.Id = usuario.Id;
                return Created($"api/clientes/{usuario.Id}", new { usuario = vm });
            }
            else
                return UnprocessableEntity(new { mensaje = "No se pudo crear el usuario" });
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
    public async Task<IActionResult> PutUsuario([FromRoute] int id, [FromBody] UsuarioVM vm)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe" });

            if (await _repo.UpdateAsync(usuario))
                return Ok(new { usuario = UsuarioVM.From(usuario) });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el usuario" });
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
    public async Task<IActionResult> DeleteUsuario([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            if (await _repo.DeleteAsync(id))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo borrar el usuario" });
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

    [HttpPatch("password/{id}")]
    public async Task<IActionResult> ChangePassword([FromRoute] int id, [FromBody] UpdatePasswordVM vm)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);

            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe" });
            
            if (vm.PasswordNuevo != null)
            {
                if (vm.PasswordActual != null && usuario.Password != HashearPassword(vm.PasswordActual))
                {
                    return BadRequest(new { mensaje = "La contraseña actual no es correcta" });
                }
            }
            else
            {
                return BadRequest(new { mensaje = "Falta la contraseña nueva" });
            }

            usuario.Password = HashearPassword(vm.PasswordNuevo);

            if (await _repo.UpdatePasswordAsync())
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo actualizar la contraseña" });
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

    [HttpPatch("{id}")]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUsuario([FromRoute] int id, [FromBody] EditUsuarioVM vm)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Username = vm.Username;
            usuario.Rol = vm.Rol;

            if (await _repo.UpdateAsync(usuario))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo actualizar el usuario" });
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

    private string HashearPassword(string password)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"] ?? "el mejor condimento del asado"),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 1000,
            numBytesRequested: 256 / 8))
        ;
    }
}