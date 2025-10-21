using Microsoft.AspNetCore.Http;

namespace Course.Bll.Service.GenralIService
{
    public interface IFileService
    {
        Task<string> UploadFileAsync (IFormFile file, string folderName);
        Task<bool> DeleteFileAsync (string fileUrl, string folder);
    }
}
