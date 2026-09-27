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
            usuario.Password = HashearPassword(usuario.Password!);

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
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] UpdatePasswordVM vm)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        try
        {
            if (vm.PasswordNuevo == null)
                return BadRequest(new { mensaje = "Falta la contraseña nueva" });

            if (vm.PasswordActual == null)
                return BadRequest(new { mensaje = "Falta la contraseña actual" });

            Usuario? usuario = await _repo.GetByIdAsync(id);

            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe" });

            if (usuario.Password != HashearPassword(vm.PasswordActual))
                return BadRequest(new { mensaje = "La contraseña actual no es correcta" });

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
    public async Task<IActionResult> UpdateUsuario([FromRoute] int id, [FromBody] UpdateDatosVM vm)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe" });

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

    [HttpPatch("avatar/{id}")]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAvatar([FromRoute] int id, [FromForm] UpdateAvatarVM vm, [FromServices] IFileService fileService)
    {
        if (id <= 0)
            return BadRequest();
            
        if (!ModelState.IsValid)
            return BadRequest(new { mensaje = "Datos incorrectos", errors = Util.ModelStateJsonError(ModelState) });

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe" });

            if (vm.AvatarFile == null)
                return BadRequest(new { mensaje = "No se envió ningún archivo" });

            if (usuario.Avatar != null)
                fileService.DeleteAvatar(usuario.Avatar);

            usuario.Avatar = await fileService.SaveAvatar(vm.AvatarFile, $"avatar_{usuario.Id}_{DateTime.Now.Ticks}");

            if (await _repo.UpdateAsync(usuario))
                return Ok(new { avatar = usuario.Avatar });
            else
                return BadRequest(new { mensaje = "No se pudo actualizar la foto de perfil" });
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

    [HttpDelete("avatar/{id}")]
    public async Task<IActionResult> DeleteAvatar([FromRoute] int id, [FromServices] IFileService fileService)
    {
        if (id <= 0)
            return BadRequest();

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe" });

            if (usuario.Avatar == null)
                return BadRequest(new { mensaje = "El usuario no tiene una foto de perfil" });;

            fileService.DeleteAvatar(usuario.Avatar);
            usuario.Avatar = null;

            if (await _repo.UpdateAsync(usuario))
                return Ok();
            else
                return BadRequest(new { mensaje = "No se pudo eliminar la foto de perfil" });
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
        if (password == null)
            throw new ClientException("No se ingresó una contraseña");
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: System.Text.Encoding.ASCII.GetBytes(_config["Salt"] ?? "el mejor condimento del asado"),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 1000,
            numBytesRequested: 256 / 8))
        ;
    }
}