using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.ErrorHandling;
using AccountManagement.Repository;
using AccountManagement.Repository.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace AccountManagement.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly IBankAccountRepository _bankAccountRepository;

        public ProductController(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository, IBankTransactionRepository bankTransactionRepository, IBankAccountRepository bankAccountRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _bankTransactionRepository = bankTransactionRepository;
            _bankAccountRepository = bankAccountRepository;
        }




        [HttpPost("Create")]
        public IActionResult Create(ProductCreateUpdateDto request)
        {
            var product = _mapper.Map<Product>(request);

            var category = _categoryRepository.FindById(request.CategoryId);
            if (category == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound,$"Category with id={request.CategoryId} does NOT exists");

            product.Category = category;

            var succeed = _productRepository.Create(product);
            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Product was created successfully");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error creating the product");

        }


        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, ProductCreateUpdateDto request)
        {
            var product = _productRepository.FindById(id);
            if (product == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={id} does NOT exist");

            var categoryForeign = _categoryRepository.FindById(request.CategoryId);
            if (categoryForeign == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Category with id={request.CategoryId} does NOT exists");


            product = _mapper.Map(request, product);

            var succeed = _productRepository.Update(product);
            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Product was updated successfully");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error updating the product");

        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.FindById(id);
            if (product == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={id} does NOT exist");

            var mapped = _mapper.Map<ProductGDto>(product);
            return Ok(mapped);
        }

        [HttpPut("InsertImage/{id}")]
        public IActionResult InsertImage(int id, [FromForm] ImageDto image)
        {
            var product = _productRepository.FindById(id);
            if (product == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={id} does NOT exist");
            using (var ms = new MemoryStream())
            {
                image.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();

                if ((fileBytes.Length > 5e+6)) throw new HttpStatusCodeException(HttpStatusCode.BadRequest,"Image size is to large , must be < 5mb ");
                product.Image = fileBytes;
            }

            var succeed = _productRepository.Update(product);
            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Product image was inserted successfully");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error inserting the product image");
        }


        [HttpGet("GetImage/{id}")]
        public IActionResult GetImage(int id)
        {
            var product = _productRepository.FindById(id);
            if (product == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={id} does NOT exist");

            if (product.Image == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={id} does NOT have an image");

            return File(product.Image, "image/png");

        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.FindById(id);
            if (product == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Product with id={id} does NOT exist");

            var succeed = _productRepository.Delete(product);
            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Product was deleted successfully");
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "There was an error deleting the product");

        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("GetProductsAndCategories")]
        public async Task<IActionResult> GetProductsCategories()
        {
            var products = await _productRepository.GetProductsAndCategories();
            return Ok(products);
        }

        /*
       
        */


    }
}

