namespace AdminPortalElixirHand.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageDirectory;

        public ImageService(IConfiguration config)
        {
            _imageDirectory = config["ImageSettings:ImageDirectory"];
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(_imageDirectory, fileName);

                // Ensure the directory exists
                if (!Directory.Exists(_imageDirectory))
                {
                    Directory.CreateDirectory(_imageDirectory);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return fileName;
            }

            throw new Exception("Image file is empty.");
        }
    }
}