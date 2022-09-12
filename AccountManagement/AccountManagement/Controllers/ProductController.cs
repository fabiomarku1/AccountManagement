﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccountManagement.Contracts;
using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AccountManagement.Data.Model;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

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
        public IActionResult Create(ProductCUDto request)
        {
            var product = _mapper.Map<Product>(request);
            product.Category = _categoryRepository.FindById(request.CategoryId);

            using (var ms = new MemoryStream())
            {
                request.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();
            }

            var succeed = _productRepository.Create(product);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromForm] ImageDto file, ProductCUDto request)
        {
            var product = _productRepository.FindById(id);
            if (product == null) return NotFound("Product with id={id} does NOT exist");
            product = _mapper.Map(request, product);

            var succeed = _productRepository.Update(product);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("GetProduct/{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.FindById(id);
            if (product == null) return NotFound("Product does NOT exist");
            return Ok(product);
        }

        [HttpPut("InsertImage/{id}")]
        public IActionResult InsertImage(int id, [FromForm] ImageDto image)
        {
            var product = _productRepository.FindById(id);
            using (var ms = new MemoryStream())
            {
                image.Image.CopyTo(ms);
                var fileBytes = ms.ToArray();

                if ((fileBytes.Length > 5e+6)) return BadRequest("Image size is to large , must be < 5mb ");
                product.Image = fileBytes;


            }

            var succeed = _productRepository.Update(product);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });

        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.FindById(id);
            if (product == null) return NotFound();

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


        [HttpGet("GetProductsAndCategories")]
        public async Task<IActionResult> GetProductsCategories()
        {
            var products = await _productRepository.GetProductsAndCategories();
            return Ok(products);
        }


        //[HttpGet("{id}")]
        //public Task<ActionResult<List<Product>>> GetCategoriesAtProducts(int id)
        //{
        //    var product=_productRepository.
        //}


    }
}

