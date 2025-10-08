namespace PMS.Helpers
{
    public class BlobHelper
    {
        private readonly IWebHostEnvironment _env;
        public BlobHelper(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folderName = "uploads")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            // Create folder if it doesn't exist
            string uploadPath = Path.Combine(_env.WebRootPath, folderName);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(uploadPath, uniqueFileName);

            // Save file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path (e.g., for use in image src)
            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }
    }
}
