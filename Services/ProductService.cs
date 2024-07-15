using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using Core.Entities;

namespace AdminPortalElixirHand.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Pagination<ProductToReturnDto>> GetProductsAsync(int pageIndex, int pageSize)
        {
            var response = await _httpClient.GetFromJsonAsync<Pagination<ProductToReturnDto>>($"/api/products?PageIndex={pageIndex}&PageSize={pageSize}") ?? throw new Exception("Response from API is null");
            return response;


        }
        public async Task<Pagination<ProductToReturnDto>> SearchProductsAsync(int pageIndex, int pageSize, string searchTerm = "")
        {
            var response = await _httpClient.GetFromJsonAsync<Pagination<ProductToReturnDto>>($"/api/products?PageIndex={pageIndex}&PageSize={pageSize}&search={searchTerm}") ?? throw new Exception("Response from API is null");
            return response;


        }


        public async Task<ProductToReturnDto> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductToReturnDto>($"/api/products/{id}") ?? throw new Exception("Response from API is null");
            return response;
        }

        public async Task<bool> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/products", productCreateDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDto productUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/products/{productUpdateDto.Id}", productUpdateDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/products/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ProductBrand>> GetProductBrandsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProductBrand>>("/api/products/brands") ?? throw new Exception("Response from API is null"); 
        }

        public async Task<IEnumerable<ProductType>> GetProductTypesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProductType>>("/api/products/types") ?? throw new Exception("Response from API is null");
        }
    }
}
