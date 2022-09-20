using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core;
using NLayer.Core.Dtos;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    [ValidateFilterAttribute]
    public class ProductController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Product> _service;
        private readonly IProductService _productService;

        public ProductController(IMapper mapper, IService<Product> service, IProductService productService)
        {
            _mapper = mapper;
            _service = service;
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var products = await _service.GetAllAsync();
            var productDto = _mapper.Map<List<ProductDto>>(products);
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200, productDto));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productDto));
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);            
            var dto = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, dto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto dto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(dto));
            var dtos = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, dtos));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto dto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(dto));

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null)
            {
                return CreateActionResult(CustomResponseDto<NoContentDto>.Fail(404, "bu id ye sahip ürün bulunamdı"));
            }
            await _service.RemoveAsync(product);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productService.GetProductWithCategory());
        }

    }
}
