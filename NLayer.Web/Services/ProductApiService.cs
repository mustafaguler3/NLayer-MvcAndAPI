using NLayer.Core.Dtos;

namespace NLayer.Web.Services
{   

    public class ProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
        {
            //var response2 = await _httpClient.GetAsync("products/GetProductsWithCategory");            

            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("products/GetProductsWithCategory");

            return response.Data;
        }

        public async Task<ProductDto> Save(ProductDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("products",dto);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            //var responseBody = await response.Content.ReadAsStringAsync();
            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

            return responseBody.Data;
        }

        public async Task<bool> UpdateAsync(ProductDto dto)
        {
            var res = await _httpClient.PutAsJsonAsync("products", dto);


            return res.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var res = await _httpClient.DeleteAsync($"products/{id}");


            return res.IsSuccessStatusCode;
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");

            return response.Data;
        }
    }
}
