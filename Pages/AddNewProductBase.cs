using AdminPortalElixirHand.Services;
using API.Dtos;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace AdminPortalElixirHand.Pages
{
    public class AddNewProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }

        public ProductCreateDto NewProduct { get; set; } = new ProductCreateDto();

        public IEnumerable<ProductType> Types { get; set; } = new List<ProductType>();
        public IEnumerable<ProductBrand> Brands { get; set; } = new List<ProductBrand>();
        public string Message { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Types = await ProductService.GetProductTypesAsync();
            Brands = await ProductService.GetProductBrandsAsync();
        }

        protected async Task HandleValidSubmit()
        {
            // Logic to handle image upload and save the product
            var isSuccess = await ProductService.CreateProductAsync(NewProduct);

            if (isSuccess)
            {
                Message = "Product added successfully!";
                Navigation.NavigateTo("/product-list");
            }
            else
            {
                Message = "Failed to add product.";
            }
        }

        protected void HandleImageUpload(InputFileChangeEventArgs e)
        {
            // Logic to handle image upload
        }

        protected void Cancel()
        {
            Navigation.NavigateTo("/product-list");
        }
    }
}
