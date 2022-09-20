using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core;
using NLayer.Core.Dtos;
using NLayer.Core.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetProductWithCategory());
        }

        public async Task<IActionResult> Save()
        {
            var categories = _categoryService.GetAllAsync();
            var catDto = _mapper.Map<List<CategoryDto>>(categories);

            ViewBag.categories = new SelectList(catDto, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto dto)
        {
            var categories = await _categoryService.GetAllAsync();
            var catDto = _mapper.Map<List<CategoryDto>>(categories);

            ViewBag.categories = new SelectList(catDto, "Id", "Name");

            if (ModelState.IsValid)
            {
                await _productService.AddAsync(_mapper.Map<Product>(dto));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            var categories = await _categoryService.GetAllAsync();
            var catDto = _mapper.Map<List<CategoryDto>>(categories);

            ViewBag.categories = new SelectList(catDto, "Id", "Name",product.CategoryId);

            return View(_mapper.Map<ProductDto>(product));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateAsync(_mapper.Map<Product>(dto));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();
            var catDto = _mapper.Map<List<CategoryDto>>(categories);

            ViewBag.categories = new SelectList(catDto, "Id", "Name", dto.CategoryId);

            return View(dto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            await _productService.RemoveAsync(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
