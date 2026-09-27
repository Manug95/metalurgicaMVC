using metalurgicaMVC.Exceptions;
using metalurgicaMVC.Interfaces;

namespace metalurgicaMVC.Services;

public class FileService : IFileService
{
    private readonly string PATH_ROOT;
    private readonly string PATH_UPLOADS;
    private readonly string PATH_ABSOLUTO_AVATARES;
    private readonly string PATH_RELATIVO_AVATARES;

    public FileService(IWebHostEnvironment env)
    {
        PATH_ROOT = env.WebRootPath;
        PATH_UPLOADS = Path.Combine(PATH_ROOT, "Uploads");
        PATH_ABSOLUTO_AVATARES = Path.Combine(PATH_UPLOADS, "Avatares");
        PATH_RELATIVO_AVATARES = Path.Combine("/Uploads", "Avatares");
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

            WriteFile(formFile, pathCompletoArchivo);

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

    public void DeleteAvatar(string path)
    {
        var pathCompletoArchivo = Path.Combine(PATH_ROOT, path.Substring(1));
        DeleteFile(pathCompletoArchivo);
    }

    private static async void WriteFile(IFormFile file, string path)
    {
        using FileStream stream = new(path, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    private static void DeleteFile(string path)
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