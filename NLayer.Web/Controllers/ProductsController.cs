using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core;
using NLayer.Core.Dtos;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductsController(CategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productService.GetProductWithCategory());
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiService.GetProductsWithCategory());
        }

        public async Task<IActionResult> Save()
        {
            var categories = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categories, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductDto dto)
        {
            
            if (ModelState.IsValid)
            {
                await _productApiService.Save(dto);

                return RedirectToAction(nameof(Index));
            }

            var catDto = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(catDto, "Id", "Name");

            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiService.GetByIdAsync(id);

            var categories = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categories, "Id", "Name",product.CategoryId);

            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(dto);

                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryApiService.GetAllAsync();

            ViewBag.categories = new SelectList(categories, "Id", "Name", dto.CategoryId);

            return View(dto);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _productApiService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
