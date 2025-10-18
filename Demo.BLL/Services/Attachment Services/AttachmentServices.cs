using Microsoft.AspNetCore.Http;

namespace Demo.BLL.Services.Attachment_Services
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly List<string> allowedExtensions = new List<string>()
        {
            ".jpg", ".jpeg", ".png", ".pdf", ".docx"
        };

        private const int MaxSize = 10 * 1024 * 1024; // 10 MB

        public bool Delete(string FileName)
        {
            if (!File.Exists(FileName)) return false;
            else
            {
                File.Delete(FileName);
                return true;
            }
        }

        public string? Upload(IFormFile File, string FolderName)
        {
            // 1.Check Extension
            var Extension = Path.GetExtension(File.FileName).ToLower();
            if (!allowedExtensions.Contains(Extension))
            {
                return null;
            }

            // 2.Check Size
            if (File.Length > MaxSize || File.Length == 0)
            {
                return null;
            }

            // 3.Get Located Folder Path
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", FolderName);

            // Create directory if not exists
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            // 4.Make Attachment Name Unique
            var fileName = $"{Guid.NewGuid()}_{File.FileName}";

            // 5.Get File Path
            var filePath = Path.Combine(FolderPath, fileName);

            // 6.Create File Stream To Copy File
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);

            // 7.Copy File
            File.CopyTo(fileStream);

            // 8.Return FileName To Store In Database
            return fileName;
        }
    }
}
