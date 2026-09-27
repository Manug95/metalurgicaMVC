namespace metalurgicaMVC.Interfaces;

public interface IFileService
{
    public Task<string> SaveAvatar(IFormFile formFile, string name);
    public void DeleteAvatar(string path);
}