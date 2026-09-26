using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.Utils;
using metalurgicaMVC.ViewModels.UsuarioViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Controllers;

public class UsuarioController(IUsuarioRepository repo) : Controller
{
    private readonly IUsuarioRepository _repo = repo;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] FiltrosUsuario filters)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MensajeError = Util.ModelStateViewError(ModelState);
            return View(new List<UsuarioVM>());
        }

        try
        {
            List<Usuario> usuarios = await _repo.ListAsync(filters);
            int cantidadUsuarios = await _repo.CountAsync(filters);

            decimal cp = Math.Ceiling((decimal)cantidadUsuarios / filters.Limit);

            ViewBag.cantPag = (int)(cp <= 0 ? 1 : cp);
            ViewBag.cantidadPaginado = filters.Limit;
            ViewBag.pagina = filters.Pagina;
            ViewBag.username = filters.Username;
            ViewBag.linkActivo = "usuarios";
            ViewBag.MensajeError = TempData["MensajeError"] as string;

            return View(UsuarioVM.FromList(usuarios));
        }
        catch (ClientException ex)
        {
            ViewBag.MensajeError = ex.Message;
        }
        catch (AppException)
        {
            ViewBag.MensajeError = "Error interno del servidor: No se pudieron traer los usuarios";
        }

        return View(new List<UsuarioVM>());
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] int id)
    {
        ViewBag.MensajeError = TempData["MensajeError"] as string;
        Usuario? usuario = await _repo.GetByIdAsync(id);

        if (usuario == null)
        {
            TempData["MensajeError"] = "El usuario no existe";
            return RedirectToAction(nameof(Index));
        }

        return View(new EditUsuarioVM() { 
            UpdatePasswordVM = new UpdatePasswordVM(),
            Id = id,
            Avatar = usuario.Avatar,
            Username = usuario.Username,
            Rol = usuario.Rol
        });
    }
}