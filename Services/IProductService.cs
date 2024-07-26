using API.Dtos;
using API.Helpers;
using Core.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace AdminPortalElixirHand.Services
{
    public interface IProductService
    {
        Task<Pagination<ProductToReturnDto>> GetProductsAsync(int pageIndex, int pageSize);
        Task<Pagination<ProductToReturnDto>> SearchProductsAsync(int pageIndex, int pageSize, string searchTerm = "");
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<bool> UpdateProductAsync(ProductUpdateDto productUpdateDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
        Task<string> UploadImageAsync(IBrowserFile file);
    }
}