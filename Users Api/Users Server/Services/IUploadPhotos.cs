namespace Users_Server.Services
{
    public interface IUploadPhotos
    {
        Task<string> UploadFile(User user, IFormFile file);
        Task DeleteAllFiles();
        Task DeleteFile(string fileName);
        Task<string> UpdateFile(User user, string fileName, IFormFile file);
    }
}