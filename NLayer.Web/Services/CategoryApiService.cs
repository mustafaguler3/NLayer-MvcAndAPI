using NLayer.Core.Dtos;

namespace NLayer.Web.Services
{
    public class CategoryApiService
    {
        private readonly HttpClient _httpClient;

        public CategoryApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var res = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<CategoryDto>>>("categories");

            return res.Data;
        }
    }
}
