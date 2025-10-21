using Microsoft.AspNetCore.Http;

namespace Course.Bll.Service.GenralIService
{
    public class FileService : IFileService
    {
        public Task<bool> DeleteFileAsync (string fileUrl, string folder)
        {
            var filePath = Path.Combine("wwwroot", folder, fileUrl);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public async Task<string> UploadFileAsync (IFormFile file, string folderName)
        {
            if (file==null||file.Length==0)
                throw new ArgumentException("File is null or empty", nameof(file));
            string name = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
            if (!Directory.Exists(Path.Combine("wwwroot", folderName)))
            {
                Directory.CreateDirectory(Path.Combine("wwwroot", folderName));
            }
            var filePath = Path.Combine("wwwroot", folderName, name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return name;
        }
    }
}
