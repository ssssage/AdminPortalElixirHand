using AdminPortalElixirHand.Services;
using API.Dtos;
using API.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPortalElixirHand.Pages
{
    public class ProductListBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductToReturnDto> Products { get; set; } = new List<ProductToReturnDto>();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 6;
        public string SearchTerm { get; set; } = string.Empty;

        public bool IsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            // Initially load all products
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            try
            {
                IsLoading = true;
                Pagination<ProductToReturnDto> pagination;

                if (string.IsNullOrWhiteSpace(SearchTerm))
                {
                    pagination = await ProductService.GetProductsAsync(CurrentPage, PageSize);
                }
                else
                {
                    pagination = await ProductService.SearchProductsAsync(CurrentPage, PageSize, SearchTerm);
                }

                if (pagination != null)
                {
                    Products = pagination.Data.ToList();
                    TotalPages = (int)Math.Ceiling(pagination.Count / (double)PageSize);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsLoading = false;
            }
        }


        protected async Task OnPageChanged(int pageIndex)
        {
            if (pageIndex >= 1 && pageIndex <= TotalPages)
            {
                CurrentPage = pageIndex;
                await LoadProducts();
            }
        }

        protected async Task SearchProducts()
        {
            CurrentPage = 1;
            await LoadProducts();
        }

        protected async Task ResetSearch()
        {
            SearchTerm = string.Empty;
            CurrentPage = 1;
            await LoadProducts();
        }

        protected async Task DeleteProduct(int productId)
        {
            var isDeleted = await ProductService.DeleteProductAsync(productId);
            if (isDeleted)
            {
                await LoadProducts(); // Reload the products to reflect the deletion
            }
            else
            {
                // Handle the error (e.g., show a message to the user)
            }
        }
    }
}
