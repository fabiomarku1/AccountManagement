using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.Model;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AccountManagement.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, IMapper mapper, IConfiguration configuration,
            ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _config = configuration;
            _categoryRepository = categoryRepository;
        }




        [HttpPost("Create")]
        public IActionResult Create(ProductViewModel request)
        {
            var product = _mapper.Map<Product>(request);

            //product.Category = _categoryRepository.GetCategory(request.CategoryId);
            // product.Category = _categoryRepository.GetCategory(request.CategoryId);

            //   var category = categoryRepository.GetCategory(request.CategoryId);
            //var cateogry

            var succeed = _productRepository.Create(product);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpPut("Update")]
        public IActionResult Update(ProductViewModel request)
        {
            var product = _mapper.Map<Product>(request);
            var succeed = _productRepository.Update(product);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(ProductViewModel request)
        {
            var product = _mapper.Map<Product>(request);
            var succeed = _productRepository.Delete(product);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("PrintDetailed")]
        public IActionResult FindAll()
        {
            //   _productRepository.test();
            return Ok(_productRepository.FindAll());
        }


        [HttpGet("getallTEst")]
        public async Task<IActionResult> GetCategorisAtProduct()
        {
            var products = await _productRepository.GetCategoryAtProducts();
            return Ok(products);
        }


        //[HttpGet("{id}")]
        //public Task<ActionResult<List<Product>>> GetCategoriesAtProducts(int id)
        //{
        //    var product=_productRepository.
        //}
    }
}

