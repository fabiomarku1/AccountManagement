using System;
using AccountManagement.Contracts;
using AccountManagement.Data.Model;
using AccountManagement.Data;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AccountManagement.Data.DTO;
using System.Net;
using AccountManagement.ErrorHandling;
using Microsoft.AspNetCore.Authorization;

namespace AccountManagement.Controllers
{
    [Route("api/Category")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpPost("Create")]
        public IActionResult Create(CategoryDto request)
        {
            var category = _mapper.Map<Category>(request);


      
                var succeed = _categoryRepository.Create(category);
          
                if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Category created successfully");
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error creating the category ");


        }
        [HttpGet("GetCategory/{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.FindById(id);

            if (category == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Category with id={id} does NOT exist");
    
            return Ok(category);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var currencies = await _categoryRepository.GetCategories();
            return Ok(currencies);
        }


        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.FindById(id);
            if (category == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound, $"Category with id={id} does NOT exist");

            var succeed = _categoryRepository.Delete(category);

            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Category was deleted successfully");
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error deleting the category ");

        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, CategoryDto request)
        {
            var category = _categoryRepository.FindById(id);
              if (category == null) throw new HttpStatusCodeException(HttpStatusCode.NotFound,$"Category with id={id} does NOT exist");

            category = _mapper.Map(request, category);

            var succeed = _categoryRepository.Update(category);

            if (succeed) throw new HttpStatusCodeException(HttpStatusCode.OK, "Category was updated successfully");
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "There was an error updating the category ");


        }
    }
}
