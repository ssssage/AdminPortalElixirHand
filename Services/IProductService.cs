using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using Core.Entities;

namespace AdminPortalElixirHand.Services
{
    public interface IProductService
    {
        Task<Pagination<ProductToReturnDto>> GetProductsAsync(int pageIndex, int pageSize);
        Task<Pagination<ProductToReturnDto>> SearchProductsAsync(int pageIndex, int pageSize, string searchTerm = "");
        Task<ProductToReturnDto> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<bool> UpdateProductAsync(ProductUpdateDto productUpdateDto);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<ProductBrand>> GetProductBrandsAsync();
        Task<IEnumerable<ProductType>> GetProductTypesAsync();
    }
}

