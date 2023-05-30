namespace Users_Server.Services
{
    public class UploadPhotos : IUploadPhotos
    {
        private readonly IWebHostEnvironment _environment;

        public UploadPhotos(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFile(User user, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null!;
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var photoUrl = $"/Uploads/{fileName}";
            user.PhotoUrl = photoUrl;
            return photoUrl;
        }


        public async Task DeleteAllFiles()
        {
            var uploadsDirectory = Path.Combine(_environment.WebRootPath, "Uploads");
            if (Directory.Exists(uploadsDirectory))
            {
                var directory = new DirectoryInfo(uploadsDirectory);
                foreach (var file in directory.GetFiles())
                {
                    await Task.Run(() => file.Delete());
                }
            }
        }

        public async Task DeleteFile(string fileName)
        {
            var uploadsDirectory = Path.Combine(_environment.WebRootPath, "Uploads");
            var filePath = Path.Combine(uploadsDirectory, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
            else return;
        }

        public async Task<string> UpdateFile(User user, string fileName, IFormFile file)
        {
            await DeleteFile(fileName);
            var photoUrl = await UploadFile(user, file);
            return photoUrl;
        }
    }
}