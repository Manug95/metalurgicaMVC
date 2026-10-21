namespace metalurgicaMVC.Interfaces;

public interface IFileService
{
    public Task<string> SaveAvatar(IFormFile formFile, string name);
    public Task<string> SaveFoto(IFormFile formFile, string name, long trabajoId);
    public void DeleteFile(string path);
}