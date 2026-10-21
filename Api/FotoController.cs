using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Models;
using metalurgicaMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace metalurgicaMVC.Api;

[ApiController]
[Route("api/fotos")]
public class FotoController(IFotoRepository repo) : ControllerBase
{
    private readonly IFotoRepository _repo = repo;

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] FotoVM viewModel, [FromServices] IFileService fileService)
    {
        if (viewModel.FotosFiles == null || viewModel.FotosFiles.Count == 0)
            return BadRequest(new { mensaje = $"No se recibieron las fotos" });

        string fotoURL;
        try
        {
            List<Foto> fotos = [];
            foreach(var fotoFile in viewModel.FotosFiles)
            {
                if (fotoFile.ContentType.Contains("image/"))
                {
                    fotoURL = await fileService.SaveFoto(fotoFile, $"foto_{Guid.NewGuid()}", viewModel.TrabajoId);
                    fotos.Add(new Foto()
                    {
                        TrabajoId = viewModel.TrabajoId,
                        Ruta = fotoURL
                    });
                }
            }
            await _repo.BulkCreateAsync(fotos);

            return Ok(FotoVM.FromList(fotos));
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
    public async Task<IActionResult> Delete([FromRoute] long id, [FromServices] IFileService fileService)
    {
        if (id <= 0)
            return BadRequest(new { mensaje = "id de foto incorrecta" });

        try
        {
            Foto? foto = await _repo.GetByIdAsync(id);

            if (foto == null || foto.Ruta == null)
                return NotFound(new { mensaje = "La foto no existe" });

            if (!await _repo.DeleteAsync(id))
                return BadRequest(new { mensaje = "No se pudo borrar la foto" });

            fileService.DeleteFile(foto.Ruta);

            return Ok();
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