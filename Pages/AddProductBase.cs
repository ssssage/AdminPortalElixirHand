using AdminPortalElixirHand.Services;
using API.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

namespace AdminPortalElixirHand.Pages
{
    public class AddProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IOptions<AppSettings> AppSettings { get; set; }

        public ProductCreateDto ProductCreateDto { get; set; } = new ProductCreateDto();
        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();
        public List<ProductBrand> ProductBrands { get; set; } = new List<ProductBrand>();

        [Inject]
        public NavigationManager Navigation { get; set; }

        public bool IsLoading { get; set; } = true;
        private IBrowserFile? selectedImage;
        private string imagePath = "images/products";

        protected override async Task OnInitializedAsync()
        {
            ProductTypes = (await ProductService.GetProductTypesAsync()).ToList();
            ProductBrands = (await ProductService.GetProductBrandsAsync()).ToList();
            IsLoading = false;
        }

        protected async Task HandleValidSubmit()
        {
            if (selectedImage != null)
            {
                var imageName = await SaveImageAsync(selectedImage);
                ProductCreateDto.PictureUrl = $"{imagePath}/{imageName}";
            }

            var success = await ProductService.CreateProductAsync(ProductCreateDto);
            if (success)
            {
                Navigation.NavigateTo("/product-list");
            }
            else
            {
                // Handle error (e.g., show error message)
            }
        }

        private async Task<string> SaveImageAsync(IBrowserFile file)
        {
            var ext = Path.GetExtension(file.Name).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
            {
                throw new Exception("Invalid image file format. Only .jpg and .png are allowed.");
            }

            var imageName = file.Name;

            string baseDir = Environment.GetEnvironmentVariable("IMAGE_SAVE_PATH");

            if (string.IsNullOrEmpty(baseDir))
            {
                throw new Exception("IMAGE_SAVE_PATH environment variable is not set.");
            }

            var path = Path.Combine(baseDir, "Content", "images", "products", imageName);

            if (File.Exists(path))
            {
                return imageName;
            }

            await using var fs = new FileStream(path, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);

            return imageName;
        }

        protected async void HandleImageUpload(InputFileChangeEventArgs e)
        {
            selectedImage = e.File;

            var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(selectedImage.Name).ToLowerInvariant();
            var validMimeTypes = new[] { "image/jpeg", "image/png" };
            if (!validExtensions.Contains(ext) || !validMimeTypes.Contains(selectedImage.ContentType.ToLowerInvariant()))
            {
                Console.WriteLine("Invalid file type. Only .jpg and .png files are allowed.");
                selectedImage = null;
                return;
            }

            var imageName = await ProductService.UploadImageAsync(selectedImage);
            ProductCreateDto.PictureUrl = $"{imagePath}/{imageName}";

            StateHasChanged();
        }

        protected void Cancel()
        {
            Navigation.NavigateTo("/product-list");
        }

        public string FullImageUrl => $"{AppSettings.Value.BaseUrl}Content/{ProductCreateDto.PictureUrl}";
    }
}
