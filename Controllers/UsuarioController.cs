using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels.UsuarioViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

[Authorize]
public class UsuarioController(IUsuarioRepository repo, IConfiguration config) : Controller
{
    private readonly IUsuarioRepository _repo = repo;
    private readonly IConfiguration _config = config;

    [HttpGet]
    [Authorize(Policy = "ADMIN")]
    public async Task<IActionResult> Index([FromQuery] FiltrosUsuario filters)
    {
        List<Usuario> usuarios = [];
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<UsuarioVM>());
        }

        try
        {
            int cantidadUsuarios = await _repo.CountAsync(filters);
            if (cantidadUsuarios > 0)
                usuarios = await _repo.ListAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadUsuarios / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.username = filters.Username;
            ViewBag.linkActivo = "usuarios";
            ViewBag.MensajeError = TempData["MensajeError"] as string;
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los usuarios";
        }

        return View(UsuarioVM.FromList(usuarios));
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromServices] IUserService userService)
    {
        if (id <= 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", "Usuario inexistente");
        }

        var userId = userService.GetUserId(User);
        if (!userId.HasValue || (!User.IsInRole("ADMIN") && userId.Value != id))
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View("Unauthorized");
        }

        try
        {
            Usuario? usuario = await _repo.GetByIdAsync(id);
            if (usuario == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return View("NotFound");
            }

            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(new EditUsuarioVM() { 
                Id = id,
                UpdateDatosVM = new UpdateDatosVM()
                {
                    Username = usuario.Username,
                    Rol = usuario.Rol,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido
                },
                UpdatePasswordVM = new UpdatePasswordVM(),
                UpdateAvatarVM = new UpdateAvatarVM()
                {
                    Avatar = usuario.Avatar
                }
            });
        }
        catch (ClientException ex)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            return View("BadRequest", ex.Message);
        }
        catch (AppException ex)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return View("InternalServerError", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult IniciarSesion(string returnUrl)
    {
        ViewBag.MensajeError = TempData["MensajeError"] as string;
        var vm = new LoginVM();
        if (!string.IsNullOrWhiteSpace(returnUrl))
            vm.ReturnUrl = returnUrl;
        return View("Login", vm);
    }

    [AllowAnonymous]
    public IActionResult Restringido()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View("Unauthorized");
    }

    [HttpGet]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(IniciarSesion));
    }
}