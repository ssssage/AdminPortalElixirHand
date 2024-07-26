﻿using AdminPortalElixirHand.Services;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AdminPortalElixirHand.Pages
{
    public class EditProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public ProductUpdateDto ProductUpdateDto { get; set; } = new ProductUpdateDto();
        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();
        public List<ProductBrand> ProductBrands { get; set; } = new List<ProductBrand>();

        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public bool IsLoading { get; set; } = true;
        private IBrowserFile? selectedImage;
        private string imagePath = "images/products";

        protected override async Task OnInitializedAsync()
        {
            var product = await ProductService.GetProductByIdAsync(Id);
            Mapper.Map(product, ProductUpdateDto);

            ProductTypes = (await ProductService.GetProductTypesAsync()).ToList();
            ProductBrands = (await ProductService.GetProductBrandsAsync()).ToList();
            IsLoading = false;
        }

        private string GetShortenedPictureUrl(string fullUrl)
        {
            return fullUrl.Replace("http://localhost:5000/", string.Empty);
        }

        public string ShortenedPictureUrl
        {
            get => ProductUpdateDto.PictureUrl.Replace("Content/", string.Empty);
            set => ProductUpdateDto.PictureUrl = value;
        }

        protected void HandleImageUrlChange(ChangeEventArgs e)
        {
            ShortenedPictureUrl = e.Value.ToString();
        }

        protected async Task HandleValidSubmit()
        {
            if (selectedImage != null)
            {
                var imageName = await SaveImageAsync(selectedImage);
                ProductUpdateDto.PictureUrl = $"{imagePath}/{imageName}";
            }
            else
            {
                ProductUpdateDto.PictureUrl = ShortenedPictureUrl;
            }

            var success = await ProductService.UpdateProductAsync(ProductUpdateDto);
            if (success)
            {
                Navigation.NavigateTo("/product-list");
            }
            else
            {
                // Handle error (show error message)
            }
        }

        protected void Cancel()
        {
            Navigation.NavigateTo("/product-list");
        }

        private async Task<string> SaveImageAsync(IBrowserFile file)
        {
            var ext = Path.GetExtension(file.Name);
            var imageName = file.Name;

            // Check if file with same name already exists
            var path = Path.Combine("C:\\Users\\elixi\\Desktop\\Code\\eCommerce-Services\\API\\Content\\images\\products", imageName);

            if (File.Exists(path))
            {
                // If file exists, return the existing file name
                return imageName;
            }

            // Save the new image
            await using var fs = new FileStream(path, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);

            return imageName;
        }

        protected async void HandleImageUpload(InputFileChangeEventArgs e)
        {
            selectedImage = e.File;

            // Validate file extension
            var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(selectedImage.Name).ToLowerInvariant();

            // Validate MIME type
            var validMimeTypes = new[] { "image/jpeg", "image/png" };
            if (!validExtensions.Contains(ext) || !validMimeTypes.Contains(selectedImage.ContentType.ToLowerInvariant()))
            {
                // Display an error message (replace with your own error handling logic)
                Console.WriteLine("Invalid file type. Only .jpg and .png files are allowed.");
                selectedImage = null;
                return;
            }

            var imageName = await SaveImageAsync(selectedImage);
            ProductUpdateDto.PictureUrl = $"{imagePath}/{imageName}";

            StateHasChanged(); // This will trigger a re-render to update the image on the UI
        }



        public string FullImageUrl => $"http://localhost:5000/Content/{ProductUpdateDto.PictureUrl}";
    }
}


