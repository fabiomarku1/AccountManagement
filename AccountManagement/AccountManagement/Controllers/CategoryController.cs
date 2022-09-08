using AccountManagement.Contracts;
using AccountManagement.Data.Model;
using AccountManagement.Data;
using AccountManagement.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AccountManagement.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _config = configuration;
        }


        [HttpPost("Create")]
        public IActionResult Create(CategoryViewModel request)
        {
            var category = _mapper.Map<Category>(request);
            var succeed = _categoryRepository.Create(category);


            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }


        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var currencies = await _categoryRepository.GetCategories();
            return Ok(currencies);
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(CategoryViewModel request)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = _categoryRepository.GetCategoryId(request);


            var succeed = _categoryRepository.Delete(category);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpPut("Update")]
        public IActionResult Update(CategoryViewModel request)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = _categoryRepository.GetCategoryId(request);

            var succeed = _categoryRepository.Update(category);
            return succeed ? Ok(new { Result = true }) : Ok(new { Result = false });
        }

        [HttpGet("PrintDetailed")]
        public IActionResult FindAll()
        {
            return Ok(_categoryRepository.FindAll());
        }
    }
}
