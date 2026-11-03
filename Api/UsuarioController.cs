using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels.UsuarioViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/usuarios")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsuarioController(
    IUsuarioRepository repo, 
    IConfiguration config,
    IUserService userService
) : ControllerBase
{
    private readonly IUsuarioRepository _repo = repo;
    private readonly IConfiguration _config = config;
    private readonly IUserService _userService = userService;

    [HttpGet("{id}")]
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

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
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Get([FromQuery] FiltrosUsuario filters)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        List<Usuario> usuarios = [];
        try
        {
            int cantidadUsuarios = await _repo.CountAsync(filters);
            if (cantidadUsuarios > 0)
                usuarios = await _repo.ListAsync(filters);

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
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Post([FromBody] UsuarioVM vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            Usuario usuario = Usuario.From(vm);
            usuario.Password = _userService.HashearPassword(usuario.Password!, _config);

            if ((await _repo.CreateAsync(usuario)) > 0)
            {
                vm.Id = usuario.Id;
                vm.Activo = true;
                return Created($"api/usuarios/{usuario.Id}", new { usuario = vm });
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
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UsuarioVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe" });

            usuario.UpdateFrom(vm);

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
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

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
    public async Task<IActionResult> UpdatePassword([FromRoute] int id, [FromBody] UpdatePasswordVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        var userId = _userService.GetUserId(User);
        if (!userId.HasValue || (!User.IsInRole("ADMIN") && userId.Value != id))
            return Unauthorized(new { mensaje = "Sin permisos" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        try
        {
            if (vm.PasswordNuevo == null)
                return BadRequest(new { mensaje = "Falta la contraseña nueva" });

            if (vm.PasswordActual == null)
                return BadRequest(new { mensaje = "Falta la contraseña actual" });

            Usuario? usuario = await _repo.GetByIdAsync(id);

            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe" });

            if (usuario.Password != _userService.HashearPassword(vm.PasswordActual, _config))
                return BadRequest(new { mensaje = "La contraseña actual no es correcta" });

            usuario.Password = _userService.HashearPassword(vm.PasswordNuevo, _config);

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
    public async Task<IActionResult> UpdateUsuario([FromRoute] int id, [FromBody] UpdateDatosVM vm)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        var userId = _userService.GetUserId(User);
        var esAdmin = User.IsInRole("ADMIN");
        if (!userId.HasValue || (!esAdmin && userId.Value != id))
            return Unauthorized(new { mensaje = "Sin permisos" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe" });

            usuario.Username = vm.Username;
            usuario.Nombre = vm.Nombre;
            usuario.Apellido = vm.Apellido;
            if (esAdmin && !string.IsNullOrWhiteSpace(vm.Rol))
                usuario.Rol = vm.Rol;

            if (await _repo.UpdateAsync(usuario))
            {
                if (userId.Value == id)
                {
                    if (User.Identity is ClaimsIdentity identity)
                    {
                        var claimUsername = identity.FindFirst(ClaimTypes.Name);
                        var claimFullName = identity.FindFirst("FullName");
                        var claimRol = identity.FindFirst(ClaimTypes.Role);
                        if (claimFullName != null)
                        {
                            identity.RemoveClaim(claimFullName);
                            var newClaim = new Claim("FullName", $"{usuario.Apellido}, {usuario.Nombre}");
                            identity.AddClaim(newClaim);
                        }
                        if (claimUsername != null)
                        {
                            identity.RemoveClaim(claimUsername);
                            var newClaim = new Claim(ClaimTypes.Name, usuario.Username ?? "Usuario");
                            identity.AddClaim(newClaim);
                        }
                        if (claimRol != null)
                        {
                            identity.RemoveClaim(claimRol);
                            var newClaim = new Claim(ClaimTypes.Role, usuario.Rol ?? "");
                            identity.AddClaim(newClaim);
                        }
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity));
                    }
                }
                return Ok();
            }
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
        catch (Exception)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
    }

    [HttpPatch("avatar/{id}")]
    public async Task<IActionResult> UpdateAvatar([FromRoute] int id, [FromForm] UpdateAvatarVM vm, [FromServices] IFileService fileService)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        var userId = _userService.GetUserId(User);
        if (!userId.HasValue || (!User.IsInRole("ADMIN") && userId.Value != id))
            return Unauthorized(new { mensaje = "Sin permisos" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (vm.AvatarFile == null)
            return BadRequest(new { mensaje = "No se envió ningún archivo" });
            
        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe" });

            if (usuario.Avatar != null)
                fileService.DeleteFile(usuario.Avatar);

            usuario.Avatar = await fileService.SaveAvatar(vm.AvatarFile, $"avatar_{usuario.Id}_{DateTime.Now.Ticks}");

            if (await _repo.UpdateAsync(usuario))
            {
                if (userId.Value == id)
                {
                    if (User.Identity is ClaimsIdentity identity)
                    {
                        var claimAvatar = identity.FindFirst(ClaimTypes.Uri);
                        if (claimAvatar != null)
                        {
                            identity.RemoveClaim(claimAvatar);
                            var newClaim = new Claim("FullName", usuario.Avatar);
                            identity.AddClaim(newClaim);
                        }
                    }
                }
                return Ok(new { avatar = usuario.Avatar });
            }
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
        catch (Exception)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
    }

    [HttpDelete("avatar/{id}")]
    public async Task<IActionResult> DeleteAvatar([FromRoute] int id, [FromServices] IFileService fileService)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id incorrecta" });

        var userId = _userService.GetUserId(User);
        if (!userId.HasValue || (!User.IsInRole("ADMIN") && userId.Value != id))
            return Unauthorized(new { mensaje = "Sin permisos" });

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "El usuario no existe" });

            if (usuario.Avatar == null)
                return BadRequest(new { mensaje = "El usuario no tiene una foto de perfil" });

            fileService.DeleteFile(usuario.Avatar);
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

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginVM login)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (login.Username == null || login.Password == null)
            return BadRequest(new { mensaje = "Falta el nombre de usuario o la contraseña" });

        try
        {
            Usuario? user = await _repo.GetByUsernameAsync(login.Username);
            string hashed = _userService.HashearPassword(login.Password, _config);
            if (user == null || user.Password != hashed)
                return Unauthorized(new { mensaje = "Nombre de usuario o clave incorrecto(s)" });

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username ?? "Usuario"),
                new(ClaimTypes.Uri, user.Avatar ?? ""),
                new(ClaimTypes.Role, user.Rol ?? ""),
                new("FullName", $"{user.Apellido}, {user.Nombre}"),
                new("id", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["TokenAuthentication:SecretKey"] ?? "jwtkey"));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["TokenAuthentication:Issuer"],
                audience: _config["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60*10),
                signingCredentials: credenciales
            );

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
        catch (ClientException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (AppException)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
        catch (ArgumentException)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
        catch (SecurityTokenEncryptionFailedException)
        {
            return StatusCode(500, new { mensaje = "Error interno del servidor" });
        }
    }
}