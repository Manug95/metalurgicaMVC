using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;

namespace metalurgicaMVC.Services;

public class FileService : IFileService
{
    private readonly string PATH_ROOT;
    private readonly string PATH_UPLOADS;
    private readonly string PATH_ABSOLUTO_AVATARES;
    private readonly string PATH_RELATIVO_AVATARES;
    private readonly string PATH_ABSOLUTO_FOTOS;
    private readonly string PATH_RELATIVO_FOTOS;

    public FileService(IWebHostEnvironment env)
    {
        PATH_ROOT = env.WebRootPath;
        PATH_UPLOADS = Path.Combine(PATH_ROOT, "Uploads");
        PATH_ABSOLUTO_AVATARES = Path.Combine(PATH_UPLOADS, "Avatares");
        PATH_RELATIVO_AVATARES = Path.Combine("/Uploads", "Avatares");
        PATH_ABSOLUTO_FOTOS = Path.Combine(PATH_UPLOADS, "Fotos");
        PATH_RELATIVO_FOTOS = Path.Combine("/Uploads", "Fotos");
    }

    public async Task<string> SaveAvatar(IFormFile formFile, string name)
    {
        try
        {
            if (!Directory.Exists(PATH_UPLOADS))
                Directory.CreateDirectory(PATH_UPLOADS);

            if (!Directory.Exists(PATH_ABSOLUTO_AVATARES))
                Directory.CreateDirectory(PATH_ABSOLUTO_AVATARES);

            string fileName = name + Path.GetExtension(formFile.FileName);
            string pathCompletoArchivo = Path.Combine(PATH_ABSOLUTO_AVATARES, fileName);

            Write(formFile, pathCompletoArchivo);

            return Path.Combine(PATH_RELATIVO_AVATARES, fileName);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("Falta la ruta de la imagen");
        }
        catch (ArgumentException)
        {
            throw new ClientException("Los datos son incorrectos");
        }
    }

    public async Task<string> SaveFoto(IFormFile formFile, string name, long trabajoId)
    {
        try
        {
            if (!Directory.Exists(PATH_UPLOADS))
                Directory.CreateDirectory(PATH_UPLOADS);

            if (!Directory.Exists(PATH_ABSOLUTO_FOTOS))
                Directory.CreateDirectory(PATH_ABSOLUTO_FOTOS);

            string pathFotosDeTrabajo = Path.Combine(PATH_ABSOLUTO_FOTOS, trabajoId.ToString());
            if (!Directory.Exists(pathFotosDeTrabajo))
                Directory.CreateDirectory(pathFotosDeTrabajo);

            string fileName = name + Path.GetExtension(formFile.FileName);
            string pathCompletoArchivo = Path.Combine(pathFotosDeTrabajo, fileName);

            Write(formFile, pathCompletoArchivo);

            return Path.Combine(PATH_RELATIVO_FOTOS, trabajoId.ToString(), fileName);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("Falta la ruta de la foto");
        }
        catch (ArgumentException)
        {
            throw new ClientException("Los datos son incorrectos");
        }
    }

    public void DeleteFile(string path)
    {
        var pathCompletoArchivo = Path.Combine(PATH_ROOT, path.Substring(1));
        Delete(pathCompletoArchivo);
    }

    private static async void Write(IFormFile file, string path)
    {
        using FileStream stream = new(path, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    private static void Delete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (DirectoryNotFoundException ex)
        {
            throw new AppException("El directorio del archivo no existe", ex);
        }
        catch (PathTooLongException ex)
        {
            throw new AppException("La ruta del archivo es muy larga", ex);
        }
        catch (IOException ex)
        {
            throw new AppException("No se pudo escribir el archivo", ex);
        }
        catch (ArgumentNullException)
        {
            throw new ClientException("Falta la ruta de la imagen");
        }
        catch (ArgumentException)
        {
            throw new ClientException("Los datos son incorrectos");
        }
        catch (NotSupportedException ex)
        {
            throw new AppException("Operación no soportada", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new AppException("El sistema no tiene permisos para borrar el archivo", ex);
        }
    }
}