using AdminPortalElixirHand.Services;
using API.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Components;

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
        public NavigationManager Navigation { get; set; }

        public bool IsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            var productToReturnDto = await ProductService.GetProductByIdAsync(Id);
            ProductUpdateDto = new ProductUpdateDto
            {
                Id = productToReturnDto.Id,
                Name = productToReturnDto.Name,
                Description = productToReturnDto.Description,
                Price = productToReturnDto.Price,
                PictureUrl = GetShortenedPictureUrl(productToReturnDto.PictureUrl),
                ProductType = productToReturnDto.ProductType,
                ProductBrand = productToReturnDto.ProductBrand
            };

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
            //set => ProductUpdateDto.PictureUrl = string.Empty;
        }

        protected void HandleImageUrlChange(ChangeEventArgs e)
        {
            ShortenedPictureUrl = e.Value.ToString();
        }

        protected async Task HandleValidSubmit()
        {
            ProductUpdateDto.PictureUrl = ShortenedPictureUrl;
            
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
    }
}
