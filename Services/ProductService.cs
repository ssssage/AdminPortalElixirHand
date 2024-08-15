using System.Net.Http.Headers;
using System.Text.Json;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Components.Forms;

namespace AdminPortalElixirHand.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public IMapper Mapper { get; set; }

        public ProductService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }


        public async Task<Pagination<ProductToReturnDto>> GetProductsAsync(int pageIndex, int pageSize)
        {
            _httpClient.DefaultRequestHeaders.Add("Cookie", ".AspNetCore.Cookies=" + _tokenProvider); 
            var response = await _httpClient.GetFromJsonAsync<Pagination<ProductToReturnDto>>($"/api/products?PageIndex={pageIndex}&PageSize={pageSize}") ?? throw new Exception("Response from API is null");
            return response;
        }

        public async Task<Pagination<ProductToReturnDto>> SearchProductsAsync(int pageIndex, int pageSize, string searchTerm = "")
        {
            var response = await _httpClient.GetFromJsonAsync<Pagination<ProductToReturnDto>>($"/api/products?PageIndex={pageIndex}&PageSize={pageSize}&search={searchTerm}") ?? throw new Exception("Response from API is null");
            return response;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Product>($"/api/products/admin/{id}") ?? throw new Exception("Response from API is null");
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

        public async Task<string> UploadImageAsync(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)); // 10MB max size
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            var response = await _httpClient.PostAsync("/api/products/upload-image", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseContent);
            // Use "pictureUrl" instead of "PictureUrl" to match the camelCase convention
            return json.RootElement.GetProperty("pictureUrl").GetString();
        }
    }
}